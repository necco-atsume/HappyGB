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

		//Setters update their respective tile pixbufs.

		public SurfaceBlitter()
		{

		}

		public void DrawTileScan(Image surface, int tile, int x, int scan, int palette) 
		{ 
			throw new NotImplementedException();
		}

		public void UpdateTile() {} //TODO: Args. Copy to each tile cache.
		public void UpdateAllTiles() { }

		private void UpdateTileCache()
		{

		}

	}
}

