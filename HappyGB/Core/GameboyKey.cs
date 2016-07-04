using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Core
{
    /// <summary>
    /// The current button state for the gameboy gamepad, represented as flags.
    /// Note: This doesn't map to anything in-memory, since the gameboy reads the input stuff by querying the
    /// input state in two steps.
    /// </summary>
    [Flags]
    public enum GameboyKey
    {
        /// <summary>
        /// The "A" button.
        /// </summary>
        A = 1, 

        /// <summary>
        /// The "B" button.
        /// </summary>
        B = 2,

        /// <summary>
        /// The "Up" direction on the D pad.
        /// </summary>
        Up = 4, 
        
        /// <summary>
        /// The "Down" direction on the D pad.
        /// </summary>
        Down = 8, 
        
        /// <summary>
        /// The "Left" direction on the D pad.
        /// </summary>
        Left = 16, 

        /// <summary>
        /// The "Right" direction on the D pad.
        /// </summary>
        Right = 32,

        /// <summary>
        /// The "Select" button.
        /// </summary>
        Select = 64,

        /// <summary>
        /// The "Start" button.
        /// </summary>
        Start = 128,
        
        /// <summary>
        /// No keys pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// All keys pressed.
        /// </summary>
        All = 0xFF
    }
}
