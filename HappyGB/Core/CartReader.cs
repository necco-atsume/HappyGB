using System;
using System.IO;

namespace HappyGB.Core
{
	public class CartReader
		: IDisposable
	{
		FileStream file;
		private BinaryReader reader;

		public CartReader(string filePath)
		{
			file = File.OpenRead(filePath);
			reader = new BinaryReader(file);
		}

		public IMemoryBankController CreateMBC()
		{
			//TODO: Get 0x147 and create a MBC from that.

			return new NoMBC(this);
		}

		public CartHeader GetHeader()
		{
			throw new NotImplementedException();
		}

		public byte[] GetBuffer(int length)
		{
			return reader.ReadBytes(length);
		}

		public void Dispose()
		{
			reader.Dispose();
			file.Dispose();
		}
	}
}

