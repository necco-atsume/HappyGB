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

			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			return new NoMBC(this);
		}

		public CartHeader GetHeader()
		{
			throw new NotImplementedException();
		}

		public byte[] GetBuffer(int length)
		{
			byte[] buf = new byte[length];
			for (int i = 0; i < length; i++)
				buf[i] = reader.ReadByte();
			return buf;
		}

		public void Dispose()
		{
			reader.Dispose();
			file.Dispose();
		}
	}
}

