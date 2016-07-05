using System;
using System.Collections.Generic;
using HappyGB.Core.Cpu;
using HappyGB.Core.Graphics;
using HappyGB.Core.Input;
using HappyGB.Core.Memory;

namespace HappyGB.Core
{
    public class Gameboy
    {
        IMemoryBankController cart;
        MemoryMap mem;
        TimerController timer;
        GBZ80 cpu;

        IInputProvider input;

        //Hack: this is public for now
        public GraphicsController gfx
        {
            get;
            private set;
        }

        /// <summary>
        /// FIXME: Hack: This is public for the debug view. 
        /// </summary>
        public GBZ80 Cpu
        {
            get { return cpu; }
        }
        public Gameboy(IInputProvider input)
        {
            CartReader cartReader = new CartReader("cpu_instrs.gb");
            cart = cartReader.CreateMBC();
            cartReader.Dispose();
            gfx = new GraphicsController();
            timer = new TimerController();
            mem = new MemoryMap(cart, gfx, timer, input);
            cpu = new GBZ80(mem);
            this.input = input;
        }

        public void Initialize()
        {
            cpu.Reset();
            //TODO: Other init code here. Set graphics/io to default states.
        }

        public void RunOneFrame()
        {
            cpu.Run(gfx, timer);

        }

        public ISurface GetSurface() //FIXME: Should be property.
        {
            return gfx.Surface;
        }

        //Temporary function for generating the inscruction frequency histogram. This should be removed.
        public IEnumerable<string> DbgDumpInstructionHistogram()
        {
            for (int i = 0; i < cpu.InstructionFrequencyCount.Length; i++)
            {
                //Make the opcode
                string instr = i.ToString() + ",0x";
                if (i > 0xFF)
                {
                    instr += "CB 0x";
                }
                instr += (i > 0xFF? i - 0xFF: i).ToString("X") + ",";
                instr += cpu.InstructionFrequencyCount[i].ToString();
                yield return instr;
            }
        }
    }
}

