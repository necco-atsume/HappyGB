// /* 
//  * Copyright © 2013 Corey Eckelman <corey@coreyeckelman.com>
//  * This work is free. You can redistribute it and/or modify it under the
//  * terms of the Do What The Fuck You Want To Public License, Version 2,
//  * as published by Sam Hocevar. See http://www.wtfpl.net/ for more details. 
//  */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HappyGB.Core;

namespace HappyGB.Xna
{
	public class GbGame
		: Game
	{
		Gameboy gb;

		private GraphicsDeviceManager graphics;

		private SpriteBatch spriteBatch;

		public GbGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.Reach;
			graphics.PreferredBackBufferWidth = 160;
			graphics.PreferredBackBufferHeight = 144;

			this.IsFixedTimeStep = true;
			this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 60.0);

			gb = new Gameboy();
		}

		protected override void Initialize()
		{
			gb.Initialize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			gb.GetSurface().Initialize(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			gb.RunOneFrame();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			spriteBatch.Draw(gb.GetSurface().Surface, Vector2.Zero, Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}

	}
}

