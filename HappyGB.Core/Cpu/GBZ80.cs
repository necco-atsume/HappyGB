using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HappyGB.Core.Graphics;
using HappyGB.Core.Memory;

namespace HappyGB.Core.Cpu
{
    /// <summary>
    /// Gameboy Z80 CPU.
    /// </summary>
    public partial class GBZ80
    {
        private RegisterGroup R;
        private MemoryMap M;

        private bool cpuInterruptEnable;
        private bool halted;

        private ulong localTickCount;

        /// <summary>
        /// The frequency count of instructions. 
        /// Used for debugging, to get which instructions are executed less / more frequently, so that we know
        /// which ones are potentially problematic (executed a few times, when the emulator is relatively stable)
        /// or potentially performance bottlenecks (executed millions of times a frame).
        /// </summary>
        public long[] InstructionFrequencyCount
        {
            get; private set;
        }

        private HashSet<ushort> breakpoints = new HashSet<ushort>();

        private Stack<ushort> stackTrace = new Stack<ushort>();

        //private void dumpstate()...

        public RegisterGroup Registers
        {
            get { return R; }
        }

        public GBZ80(MemoryMap memoryMap)
        {
            R = new RegisterGroup();
            M = memoryMap;
            localTickCount = 0;

            AddBreakpoints();

            InstructionFrequencyCount = new long[512];
        }

        public void Reset()
        {
            R.af = 0x01B0;
            R.UpdateFlagsFromAF();
            R.bc = 0x0013;
            R.de = 0x00D8;
            R.hl = 0x014D;
            R.sp = 0xFFFE;

            if (M.BiosEnabled == false)
                R.pc = 0x0100;
            else R.pc = 0x0000;

            cpuInterruptEnable = false;

            //Timers.
            M[0xFF05] = M[0xFF06] = M[0xFF07] = 0x00;

            //TODO: Sound

            //Graphics.
            M[0xFF40] = 0x91; //LCDC
            M[0xFF42] = M[0xFF43] = M[0xFF45] = 0x00;  //scx scy lyc
            M[0xFF47] = 0xFC; //Palettes.
            M[0xFF48] = 0xFF;
            M[0xFF49] = 0xFF;
            M[0xFF4A] = M[0xFF4B] = 0x00; //wx wy

            M[0xFFFF] = 0x00; //IE
        }

        public bool Run(GraphicsController graphics, TimerController interrupts)
        {
            //Run until an interrupt is hit.
            //Then handle that interrupt.
            ulong startTicks;
            ulong ticksSinceLastYield = localTickCount;
            var lcdInterrupt = InterruptType.None;

            halted = false; 

            bool shouldYield = false;

            while (true) {
                var pcOld = R.pc;
                ///Hack: Breakpoints will be implemented actually sometime.
                if (IsAtBreakpoint())
                {
                    System.Diagnostics.Debug.WriteLine("Breakpoint at 0x{0:x4} triggered.", R.pc);
                }

                //Handle interrupts.
                if (cpuInterruptEnable && ((M.IE & M.IF) != 0))
                {
                    halted = false;
                    var mf = (InterruptType)M.IF;
                    if (mf.HasFlag(InterruptType.VBlank))
                    {
                        //Handle VBlank
                        HandleInterrupt(InterruptType.VBlank);
                    }
                    if (mf.HasFlag(InterruptType.LCDController))
                    {
                        //Handle LCDC stat 
                        HandleInterrupt(InterruptType.LCDController);
                    }
                    if (mf.HasFlag(InterruptType.TimerOverflow))
                    {
                        //Handle timer 
                        HandleInterrupt(InterruptType.TimerOverflow);
                    }
                    if (mf.HasFlag(InterruptType.SerialComplete))
                    {
                        HandleInterrupt(InterruptType.SerialComplete);
                    }
                    if (mf.HasFlag(InterruptType.ButtonPress))
                    {
                        //Handle bpress 
                        HandleInterrupt(InterruptType.ButtonPress);
                    }
                }

                startTicks = localTickCount;
                if (halted)
                    Tick(4);
                else
                    Execute(); //Run one instruction.

                //Update the lcd controller.
                int instructionTicks = (int)(localTickCount - startTicks);
                lcdInterrupt = graphics.Update(instructionTicks);

                if (lcdInterrupt.HasFlag(InterruptType.VBlank)) //Yield on vblank so we can draw the next frame.
                    shouldYield = true;

                M.IF |= (byte)lcdInterrupt;

                //Handle interrupts in priority order.
                M.IF |= (byte)interrupts.Tick(instructionTicks);

                if (shouldYield)
                    return true;
            }
            throw new InvalidOperationException("This shouldn't have gotten here.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick(ulong ticks)
        {
            localTickCount += ticks;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte Fetch8()
        {
            byte ret = M[R.pc];
            R.pc++;
            return ret;
        }

        public ushort Fetch16()
        {
            ushort ret = M.Read16(R.pc);
            R.pc += 2;
            return ret;
        }

        public void HandleInterrupt(InterruptType interrupt)
        {
            M.IF &= (byte)(0xFF - (byte)interrupt); //Unset in IF.
            cpuInterruptEnable = false; //Unset master interrupt.
            switch (interrupt)
            {
                case InterruptType.VBlank:
                    RST(0x40);
                    break;
                case InterruptType.LCDController:
                    RST(0x48);
                    break;
                case InterruptType.TimerOverflow:
                    RST(0x50);
                    break;
                case InterruptType.SerialComplete:
                    RST(0x58);
                    break;
                case InterruptType.ButtonPress:
                    RST(0x60);
                    break;
            }
            Tick(12);
        }

        /// <summary>
        /// Checks if the list of breakpoints contains the current PC.
        /// </summary>
        /// <returns></returns>
        public bool IsAtBreakpoint()
        {
            if (breakpoints.Contains(this.R.pc))
                return true;
            else return false;
        }

        /// <summary>
        /// Adds each of the breakpoints. 
        /// </summary>
        public void AddBreakpoints()
        {
            breakpoints.Add(0x100); //Entry point for ROM
            breakpoints.Add(0x0546);
            breakpoints.Add(0x1aac);

        }

}
}

