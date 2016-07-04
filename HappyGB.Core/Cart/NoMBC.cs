using System;
using System.IO;

namespace HappyGB.Core
{
    /// <summary>
    /// A ROM-only cartridge implementation backed by a single byte[] buffer.
    /// </summary>
    public class NoMBC
        : IMemoryBankController
    {
        private byte[] rom;

        public NoMBC(CartReader cart)
        {
            rom = cart.GetBuffer(0x8000);
        }

        #region IMemoryBankController implementation

        public void Reset() { /* nothing */ }

        public byte Read8(ushort address)
        {
            if (address > 0x8000) return 0; //RAM
            else return rom[address];
        }

        public void Write8(ushort address, byte value) { /* nothing */ }

        #endregion
    }
}

