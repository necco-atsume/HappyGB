using System;
using System.IO;

namespace HappyGB.Core
{
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
			return rom[address];
		}

		public void Write8(ushort address, byte value) { /* nothing */ }

		#endregion
	}
}

