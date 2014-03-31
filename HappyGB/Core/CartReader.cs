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
            reader.BaseStream.Seek(0x143, SeekOrigin.Begin);
            byte gbType = reader.ReadByte();

            reader.BaseStream.Seek(0x146, SeekOrigin.Begin);
            byte sgbEnabled = reader.ReadByte();
            byte mbcType = reader.ReadByte();
            byte romSize = reader.ReadByte();
            byte ramSize = reader.ReadByte();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            //Set rom and ram sizes to correct kilobyte values.
            int romSizeKB, ramSizeKB;
            switch (romSize)
            {
                case 0: romSizeKB = 32; break;
                case 1: romSizeKB = 64; break;
                case 2: romSizeKB = 128; break;
                case 3: romSizeKB = 256; break;
                case 4: romSizeKB = 512; break;
                case 5: romSizeKB = 1024; break;
                case 6: romSizeKB = 2048; break;
                case 0x52: romSizeKB = 1152; break;
                case 0x53: romSizeKB = 1280; break;
                case 0x54: romSizeKB = 1536; break;
                default: throw new InvalidDataException("Invalid ROM size value in header.");
            }

            switch (ramSize)
            {
                case 0: ramSizeKB = 0; break;
                case 1: ramSizeKB = 2; break;
                case 2: ramSizeKB = 8; break;
                case 3: ramSizeKB = 32; break;
                case 4: ramSizeKB = 128; break;
                default: throw new InvalidDataException("Invalid RAM size value in header.");
            }

            switch (mbcType)
            {
                case 0x00:
                    return new NoMBC(this);
                case 0x01:
                case 0x02:
                case 0x03:
                    return new MBC1(this, mbcType, romSizeKB, ramSizeKB);

                default: throw new Exception("MBC type unsupported or invalid.");
            }

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

