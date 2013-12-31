using System;

namespace HappyGB.Core
{
	/// <summary>
	/// A color encoded as a RGBA value.
	/// </summary>
	public enum GbColorMonochrome
		: uint 
	{
		White = 0xFFFFFFFF,
		LightGrey = 0xBBBBBBFF,
		DarkGrey = 0x666666FF,
		Black = 0x000000FF,
	}
}

