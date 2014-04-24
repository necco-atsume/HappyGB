using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Core
{
    public interface IInputProvider
    {
        bool GetInputState(GameboyKey key);
    }
}
