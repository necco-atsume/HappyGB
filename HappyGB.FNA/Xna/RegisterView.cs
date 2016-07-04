using HappyGB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core.Cpu;

namespace HappyGB.Xna
{
    public class RegisterView
        : IDebugView
    {
        private readonly GBZ80 cpu;

        public StringBuilder Message
        {
            get;
            private set;
        }

        public RegisterView(GBZ80 cpu)
        {
            this.cpu = cpu;
            this.Message = new StringBuilder();
        }

        public void Update()
        {
            //FIXME: Generates garbage!
            Message.Clear();
            var r = cpu.Registers;
            Message
                .Append(";SP=").Append(r.sp.ToString("x4"))
                .AppendLine()
                .Append(";PC=").Append(r.pc.ToString("x4"));
        }
    }
}
