using System;

namespace HappyGB.Core
{
	/// <summary>
	/// A color encoded as a RGBA value.
	/// </summary>
	public enum GbColorMonochrome
		: byte 
	{
		/*White = 0xFFFFFFFF,
		LightGrey = 0xBBBBBBFF,
		DarkGrey = 0x666666FF,
		Black = 0x000000FF,*/

		White = 0x11,
		LightGrey = 0x10,
		DarkGrey = 0x01,
		Black = 0x00,
	}
}

