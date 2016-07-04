using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HappyGB.Core.Graphics
{
	public class ScreenSurface
		: ISurface
	{
		private Color[] buffer;
		private Texture2D texture;

		public ScreenSurface()
		{
			buffer = new Color[160 * 144]; 
		}

		#region ISurface implementation

		public void Initialize(GraphicsDevice graphics)
		{
			texture = new Texture2D(graphics, 160, 144, false, SurfaceFormat.Color);
		}

		//write to dis
		public Color[] Buffer {
			get {
				return buffer;
			}
		}

		//get stuff from dis
		public Texture2D Surface {
			get {
				UpdateSurface(); //FIXME: Side effect when getting property: OK?
				return texture;
			}
		}
		#endregion

		private void UpdateSurface()
		{
			texture.SetData<Color>(buffer);
		}
	}
}

