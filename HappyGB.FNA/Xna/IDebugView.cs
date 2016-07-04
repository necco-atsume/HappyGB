using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Xna
{
    /// <summary>
    /// A debug view that's vomited onto the screen.
    /// </summary>
    interface IDebugView
    {
        StringBuilder Message
        {
            get;
        }

        void Update();
    }
}
