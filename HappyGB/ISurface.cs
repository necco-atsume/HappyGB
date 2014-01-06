using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HappyGB
{
	public interface ISurface
	{
		Color[] Buffer
		{
			get;
		}

		Texture2D Surface {
			get;
		}

		void Initialize(GraphicsDevice graphics);
	}
}

