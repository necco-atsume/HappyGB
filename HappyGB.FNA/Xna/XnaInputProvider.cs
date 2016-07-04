using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core.Input;
using Microsoft.Xna.Framework.Input;

namespace HappyGB.Xna
{
    internal class XnaInputProvider
        : IInputProvider
    {
        private KeyboardState keys;
        public XnaInputProvider()
        {
            keys = new KeyboardState();
        }

        public void UpdateInputState()
        {
            keys = Keyboard.GetState();
        }

        public bool GetInputState(GameboyKey key)
        {
            //TODO: Make this configurable.
            if ((key == GameboyKey.A) && keys.IsKeyDown(Keys.Z))
                return true;

            if (key == GameboyKey.B && keys.IsKeyDown(Keys.X))
                return true;
            
            if ((key == GameboyKey.Up) && keys.IsKeyDown(Keys.Up))
                return true;
            if (key == GameboyKey.Down && keys.IsKeyDown(Keys.Down))
                return true;
            if (key == GameboyKey.Left && keys.IsKeyDown(Keys.Left))
                return true;
            if (key == GameboyKey.Right && keys.IsKeyDown(Keys.Right))
                return true;
            
            if (key == GameboyKey.Select && keys.IsKeyDown(Keys.C))
                return true;

            if (key == GameboyKey.Start && keys.IsKeyDown(Keys.Enter))
                return true;

            return false;
        }
    }
}
