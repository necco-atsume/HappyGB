using System;

namespace HappyGB.Core
{
	/// <summary>
	/// Interrupt type flags.
	/// </summary>
	[Flags]
	public enum InterruptType
		: byte
	{
		None = 0x00,
		VBlank = 0x01,
		LCDController = 0x02,
		TimerOverflow = 0x04,
		SerialComplete = 0x08,
		ButtonPress = 0x10
	}
}

