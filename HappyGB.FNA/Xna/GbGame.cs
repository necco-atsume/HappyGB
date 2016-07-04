using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private SpriteFont debugFont;
        private VramViewer vramViewer;
        private XnaInputProvider input;

        private List<IDebugView> debugMessages;

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

            debugMessages = new List<IDebugView>();
            debugMessages.Add(new ButtonView(input));
            debugMessages.Add(new RegisterView(gb.Cpu));
        }

        protected override void Initialize()
        {
            gb.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Services, "");

            debugFont = content.Load<SpriteFont>("spritefont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gb.GetSurface().Initialize(GraphicsDevice);

            vramViewer.LoadContent(GraphicsDevice, content);
        }

        protected override void Update(GameTime gameTime)
        {
            input.UpdateInputState();

            gb.RunOneFrame();
            vramViewer.UpdateSurfaces();

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                this.Exit();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(gb.GetSurface().Surface, Vector2.Zero, null, Color.White, 0f, 
                Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.Draw(vramViewer.Tiles, new Vector2(160 * 2, 0), null, Color.White, 0f, 
                Vector2.Zero, 2f, SpriteEffects.None, 1f);

            //Drawing strings we create each frame is gonna create garbage which is gonna cause a gigantic pause.
            //TODO: Pull this out into debug stuff.

            //foreach (var view in debugMessages)
            for(int i = 0; i < debugMessages.Count; i++)
            {
                var view = debugMessages[i];
                view.Update();
                spriteBatch.DrawString(debugFont, view.Message, new Vector2(0, (144 * 2 + (i * 10))), Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            var instrCounts = this.gb.DbgDumpInstructionHistogram();

            File.WriteAllLines("./instrHistogram.csv", instrCounts);
                
            base.Dispose(disposing);
        }

    }
}

