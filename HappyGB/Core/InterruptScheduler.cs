using System;
using System.Runtime.CompilerServices;

namespace HappyGB.Core
{
	/// <summary>
	/// Handles timer interrupts.
	/// </summary>
	public class InterruptScheduler
	{
		//TMA / TIMA / DIV registers.
		private int timaTicks; 
		private uint divTicks;
		private int clockDivider;

		private bool enabled;
		private byte _tma;
		public byte TMA {
			get { return _tma; }
			set {
				_tma = value;
				enabled = ((0x04 & value) == value);
				switch (value & 0x03)
				{
				case 0:
					clockDivider = 1024;
					break;
				case 1:
					clockDivider = 16;
					break;
				case 2:
					clockDivider = 64;
					break;
				case 3:
					clockDivider = 256;
					break;
				}
			}
		}

		private int _tima;
		public byte TIMA {
			get {
				return (byte)(0xFF & _tima);
			}
		}

		public byte TAC {
			get;
			set;
		}

		public byte DIV {
			//div increments every 256 cycles
			//get { return (uint)(divTicks % (256 * 256)) / 256; }
			get { return (byte)((divTicks / (256 * 256)) % 256); }
			set { divTicks = 0; }
		}

		public InterruptScheduler()
		{
			TMA = 0;
			DIV = 0;
			_tima = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public InterruptType Tick(int ticks)
		{
			divTicks = (uint)unchecked(divTicks + ticks);
			timaTicks += ticks;
			if (timaTicks > clockDivider)
			{
				timaTicks %= clockDivider;
				_tima++;
				if (_tima > 0xFF)
				{
					_tima = TMA;
					return InterruptType.TimerOverflow;
				}
			}
			return InterruptType.None;
		}
	}
}

