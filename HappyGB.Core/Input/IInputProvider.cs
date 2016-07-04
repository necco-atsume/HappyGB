using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Core.Input
{
    /// <summary>
    /// A simple interface for input providers. 
    /// This is queried each time the P1 register is read.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Gets the input state for a given gameboy button.
        /// This will be called each time that the P1 register (0xFF00) is read.
        /// </summary>
        /// <param name="key">The key (A, B, Select...) to read.</param>
        /// <returns>Returns whether or not the key is pressed currently. </returns>
        bool GetInputState(GameboyKey key);
    }

    /// <summary>
    /// Extension methods for IInputProvider
    /// </summary>
    public static class IInputProviderExtensions
    {
        /// <summary>
        /// Gets the state of each of the buttons.
        /// </summary>
        /// <param name="input">The input privider to query.</param>
        /// <returns>Returns the input as a group of flags.</returns>
        public static GameboyKey GetAllInputStates(this IInputProvider input)
        {
            GameboyKey keyState = GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.A)? GameboyKey.A : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.B)? GameboyKey.B : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Up)? GameboyKey.Up : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Down)? GameboyKey.Down : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Left)? GameboyKey.Left : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Right)? GameboyKey.Right : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Start)? GameboyKey.Start : GameboyKey.None;
            keyState |= input.GetInputState(GameboyKey.Select)? GameboyKey.Select : GameboyKey.None;
            return keyState;
        }

        public static void OutputInputStates(this IInputProvider input)
        {
            Console.WriteLine("Frame: {0}", GetAllInputStates(input).ToString());
        }

    }
}
