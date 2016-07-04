using System;

namespace HappyGB.Core.Graphics
{
	/// <summary>
	/// A color in the Gameboy palette.
	/// </summary>
	public enum GbColorMonochrome
		: byte 
	{
		White = 0x00,
		LightGrey = 0x01,
		DarkGrey = 0x02,
		Black = 0x03,
	}
}

