using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

namespace HappyGB.Xna
{
    internal class XnaInputProvider
        : Core.IInputProvider
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

        public bool GetInputState(Core.GameboyKey key)
        {
            if ((key == Core.GameboyKey.A) && keys.IsKeyDown(Keys.Z))
                return true;

            if (key == Core.GameboyKey.B && keys.IsKeyDown(Keys.X))
                return true;
            
            if ((key == Core.GameboyKey.Up) && keys.IsKeyDown(Keys.Up))
                return true;
            if (key == Core.GameboyKey.Down && keys.IsKeyDown(Keys.Down))
                return true;
            if (key == Core.GameboyKey.Left && keys.IsKeyDown(Keys.Left))
                return true;
            if (key == Core.GameboyKey.Right && keys.IsKeyDown(Keys.Right))
                return true;
            
            if (key == Core.GameboyKey.Select && keys.IsKeyDown(Keys.C))
                return true;

            if (key == Core.GameboyKey.Start && keys.IsKeyDown(Keys.Enter))
                return true;

            return false;
        }
    }
}
