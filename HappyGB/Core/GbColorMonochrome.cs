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

		White = 0x00,
		LightGrey = 0x01,
		DarkGrey = 0x02,
		Black = 0x03,
	}
}

