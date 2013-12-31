using System;
using System.Runtime.CompilerServices;

namespace HappyGB.Core
{
	public class MemoryMap
	{
		private byte[] internalRam;
		private byte[] highRam;

		public byte IE, IF;

		private IMemoryBankController cart;
		private GraphicsController gfx;
		private IOController io;

		public byte this[ushort addr]
		{	
			//TODO: nested switches.
			get 
			{
				//Perform memory mapping stuff here.
				if (addr < 0x8000)
					return cart.Read8(addr); //Cart
				else if (addr < 0xA000)
					throw new NotImplementedException(); //vRam
				else if (addr < 0xC000)
					return cart.Read8(addr);
				else if (addr < 0xE000)
					return internalRam[addr - 0xC000];
				else if (addr < 0xFE00)
					return internalRam[addr - 0xE000];
				else if (addr < 0xFEFF)
					return gfx.ReadOAM8(addr); //FIXME: Dead zone is mapped to OAM.
				else 
				{
					//IO and HighRam.
					switch (addr & 0x00FF) {
					case 0xFF:
						return IE;
					case 0x0F:
						return IF;
					default:
						if(addr < 0xFF80)
							return 0;
						return highRam[addr - 0xFF80];
					}
				}
				throw new InvalidOperationException("Can't handle this memory location. This shouldn't be reached ever.");
			}

			set 
			{ 
				//Perform memory mapping stuff here.
				if (addr < 0x8000)
					cart.Write8(addr, value); //Cart
				else if (addr < 0xA000)
					gfx.WriteVRAM8(addr, value);
				else if (addr < 0xC000)
					cart.Write8(addr, value);
				else if (addr < 0xE000)
					internalRam[addr - 0xC000] = value;
				else if (addr < 0xFE00)
					internalRam[addr - 0xE000] = value;
				else if (addr < 0xFEFF)
					gfx.WriteOAM8(addr, value);
				else 
				{
					//IO and HighRam.
					switch (addr & 0x00FF) {
					case 0xFF:
						IE = value;
						break;
					case 0x0F:
						IF = value;
						break;
					default:
						if(addr < 0xFF80) // IO port but not mapped.
						{ /* ??? */}
						else highRam[addr - 0xFF80] = value; //or highram.
						break;
					}
				}
			}
		}

		public MemoryMap(IMemoryBankController cart, GraphicsController gfx, IOController io)
		{
			internalRam = new byte[0x2000];
			highRam = new byte[0x80];
			this.cart = cart;
			this.gfx = gfx;
			this.io = io;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Read8(ushort addr)
		{
			return this[addr];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write8(ushort addr, byte value)
		{
			this[addr] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort Read16(ushort addr)
		{
			//FIXME: Endianness right?
			ushort ret = 0;
			ushort vl = (ushort)this[addr];
			ushort vr = (ushort)(this[(ushort)(addr + 1)] << 8);
			ret = (ushort)(vl | vr);
			return ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write16(ushort addr, ushort value)
		{
			//FIXME: Endianness right?
			this[addr] = (byte)(value & 0xFF);
			this[(ushort)(addr + 1)] = (byte)((value >> 8) & 0xFF);
		}
	}
}

