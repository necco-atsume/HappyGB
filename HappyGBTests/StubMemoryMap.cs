using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core.Memory;

namespace HappyGBTests
{
    public class StubMemoryMap
        : IMemoryMap
    {
        private readonly byte[] buf = new byte[ushort.MaxValue + 1];

        public StubMemoryMap() { /* Blank. */ }

        public byte this[ushort addr]
        {
            get
            {
                return buf[addr];
            }

            set
            {
                buf[addr] = value;
            }
        }

        public bool BiosEnabled
        {
            get; set;
        }

        public byte IE
        {
            get;
            set;
        }

        public byte IF
        {
            get;
            set;
        }
    }
}
