using HappyGB.Core;
using System;
using System.Text;

namespace HappyGB.Xna
{

    public class ButtonView
        : IDebugView
    {
        private readonly IInputProvider input;
        private GameboyKey currentInputState;

        public StringBuilder Message
        {
            get;
            private set;
        }

        public void Update()
        {
            currentInputState = input.GetAllInputStates();
            Message.Clear();
            Message.Insert(0, currentInputState.HasFlag(GameboyKey.A) ? 'A' : ' ');
            Message.Insert(1, currentInputState.HasFlag(GameboyKey.B) ? 'B' : ' ');
            Message.Insert(2, currentInputState.HasFlag(GameboyKey.Start) ? 'S' : ' ');
            Message.Insert(3, currentInputState.HasFlag(GameboyKey.Select) ? 's' : ' ');
            Message.Insert(4, currentInputState.HasFlag(GameboyKey.Up) ? 'U' : ' ');
            Message.Insert(5, currentInputState.HasFlag(GameboyKey.Down) ? 'D' : ' ');
            Message.Insert(6, currentInputState.HasFlag(GameboyKey.Left) ? 'L' : ' ');
            Message.Insert(7, currentInputState.HasFlag(GameboyKey.Right) ? 'R' : ' ');
        }

        public ButtonView(IInputProvider input)
        {
            this.input = input;
            Message = new StringBuilder(8);
        }
    }
}
