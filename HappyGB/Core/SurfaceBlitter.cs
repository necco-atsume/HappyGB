using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HappyGB.Core
{
	/// <summary>
	/// Provides methods for blitting the surface stuff directly.
	/// Takes care of constructing and drawing the tiles.
	/// </summary>
	public class SurfaceBlitter
	{
		public enum PalletteType
		{
			BGP,
			OBP0,
			OBP1
		}

		public SurfaceBlitter()
		{

		}

		public void DrawTileScan(Graphics g, byte tileLow, byte tileHigh, int x, int y, GbPalette palette) 
		{ 
			throw new NotImplementedException();
		}

	}
}

