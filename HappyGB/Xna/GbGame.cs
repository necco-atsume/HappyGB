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
using Microsoft.Xna.Framework.Content;

namespace HappyGB.Xna
{
    public class GbGame
        : Game
    {
        Gameboy gb;

        private GraphicsDeviceManager graphics;
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private VramViewer vramViewer;
        private XnaInputProvider input;

        public GbGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.Reach;
            graphics.PreferredBackBufferWidth = (160 * 2) + (128 * 2);
            //graphics.PreferredBackBufferHeight = 144 * 2;
            graphics.PreferredBackBufferHeight = 24 * 8 * 2;

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 60.0);

            input = new XnaInputProvider();
            gb = new Gameboy(input);
            vramViewer = new VramViewer(gb.gfx);
        }

        protected override void Initialize()
        {
            gb.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Services, "");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gb.GetSurface().Initialize(GraphicsDevice);

            vramViewer.LoadContent(GraphicsDevice, content);
        }

        protected override void Update(GameTime gameTime)
        {
            input.UpdateInputState();

            gb.RunOneFrame();
            vramViewer.UpdateSurfaces();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, 
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(gb.GetSurface().Surface, Vector2.Zero, null, Color.White, 0f, 
                Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.Draw(vramViewer.Tiles, new Vector2(160 * 2, 0), null, Color.White, 0f, 
                Vector2.Zero, 2f, SpriteEffects.None, 1f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}

