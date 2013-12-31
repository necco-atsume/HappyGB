using System;

namespace HappyGB.Core
{
	public interface IMemoryBankController
	{
		void Reset();

		byte Read8(ushort address);
		void Write8(ushort address, byte value);
	}
}

