using System;
using System.Drawing;

namespace HappyGB.Core
{
	/// <summary>
	/// Provides methods for blitting the surface stuff directly.
	/// Takes care of constructing and drawing the tiles.
	/// </summary>
	public class SurfaceBlitter
	{
		private byte[] bgTiles, obp0Tiles, obp1Tiles;

		//Setters update their respective tile pixbufs.

		public SurfaceBlitter()
		{
			//setup bg, obp, ... tiles.
		}

		public void DrawTileScan(Image surface, int x, int y, int tile, int palette) { }

		public void UpdateTile() {} //TODO: Args. Copy to each tile cache.
		public void UpdateAllTiles() { }

		private void UpdateTileCache(byte[] tiles)
		{
		}

	}
}

