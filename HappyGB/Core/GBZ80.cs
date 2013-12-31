using System;
using System.Runtime.CompilerServices;

namespace HappyGB.Core
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

		private int localTickCount;

		public GBZ80(MemoryMap memoryMap)
		{
			R = new RegisterGroup();
			M = memoryMap;
			localTickCount = 0;
		}

		public void Reset()
		{
			R.af = 0x01B0;
			R.UpdateFlagsFromAF();
			R.bc = 0x0013;
			R.de = 0x00D8;
			R.hl = 0x014D;
			R.sp = 0xFFFE;
			R.pc = 0x0100;

			cpuInterruptEnable = true;
			//TODO: Set MemoryMapped Registers.
			M[0xFFFF] = 0x00;
		}

		public void Run(GraphicsController graphics, InterruptScheduler interrupts)
		{
			//Run until an interrupt is hit.
			//Then handle that interrupt.
			long startTicks;
			var lcdInterrupt = InterruptType.None;
			halted = false;
			while (true) {

				//Handle interrupts.
				if (cpuInterruptEnable && ((M.IE & M.IF) != 0))
				{
					halted = false;
					var mf = (InterruptType)M.IF;
					if (mf.HasFlag(InterruptType.VBlank))
					{
						//Handle VBlank
						M.IF &= (0xFF - (byte)InterruptType.VBlank); //Unset in IF.
						cpuInterruptEnable = false; //Unset master interrupt.

						//Now push the PC
						//and jump to the interrupt vector.

						throw new NotImplementedException();
					}
					if (mf.HasFlag(InterruptType.LCDController))
					{
						//Handle LCDC stat 
						throw new NotImplementedException();
					}
					if (mf.HasFlag(InterruptType.TimerOverflow))
					{
						//Handle timer 
						throw new NotImplementedException();
					}
					if (mf.HasFlag(InterruptType.SerialComplete))
					{
						//Handle serial 
						throw new NotImplementedException();
					}
					if (mf.HasFlag(InterruptType.ButtonPress))
					{
						//Handle bpress 
						throw new NotImplementedException();
					}
				}

				startTicks = localTickCount;

				if (halted)
					Tick(4);
				else
					Execute(); //Run one instruction.

				//Update the lcd controller.
				//FIXME: Priorities are donked up.
				lcdInterrupt = graphics.Update((int)(localTickCount - startTicks));
				M.IF |= (byte)lcdInterrupt;

				//Handle interrupts in priority order.

				//handle timer and pin io.
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Tick(int ticks)
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

	}
}

