using System;
using System.Runtime.CompilerServices;

namespace HappyGB.Core
{
	public class MemoryMap
	{
		private byte[] internalRam;
		private byte[] highRam;

		public byte IE, IF;

		public byte DMA
		{
			set {
				ushort addr = value;
				addr = (ushort)(addr << 8);
				for(ushort i = 0; i < 0xA0; i++)
					gfx.WriteOAM8((ushort)(addr + i),
						this[(ushort)(addr + i)]);
			}
		}

		private IMemoryBankController cart;
		private GraphicsController gfx;
		private InterruptScheduler timer;

		public byte this[ushort addr]
		{	
			//TODO: nested switches.
			get 
			{
				//Perform memory mapping stuff here.
				if (addr < 0x8000)
					return cart.Read8(addr); //Cart
				else if (addr < 0xA000)
					return gfx.ReadVRAM8(addr);
				else if (addr < 0xC000)
					return cart.Read8(addr);
				else if (addr < 0xE000)
					return internalRam[addr - 0xC000];
				else if (addr < 0xFE00)
					return internalRam[addr - 0xE000];
				else if (addr < 0xFEA0)
					return gfx.ReadOAM8(addr); //FIXME: Dead zone is mapped to OAM.
				else if (addr < 0xFEFF)
					return 0;
				else 
				{
					//IO and HighRam.
					switch (addr & 0x00FF) {
					case 0xFF:
						return IE;
					case 0x0F:
						return IF;

						///Timer
					case 0x04: 
						return timer.DIV;
					case 0x05: 
						return timer.TIMA;
					case 0x06: 
						return timer.TMA;
					case 0x07: 
						return timer.TAC;

						///Graphics.
					case 0x40: 
						return gfx.LCDC;
					case 0x41: 
						return gfx.STAT;
					case 0x42:
						return gfx.SCY;
					case 0x43:
						return gfx.SCX;
					case 0x44:
						return gfx.LY;
					case 0x45:
						return gfx.LYC;
					case 0x47:
						return gfx.BGP;
					case 0x48:
						return gfx.OBP0;
					case 0x49:
						return gfx.OBP1;
					case 0x4A:
						return gfx.WY;
					case 0x4B:
						return gfx.WX;
					default:
						if(addr < 0xFF80)
							return 0;
						return highRam[addr - 0xFF80];
					}
				}
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
				else if (addr < 0xFEA0)
					gfx.WriteOAM8(addr, value);
				else if (addr < 0xFEFF)
					//System.Diagnostics.Debug.WriteLine("Writing to junk address." + addr.ToString("X"));
				{}
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

						///Timer
					case 0x04:
						timer.DIV = value;
						break;
					case 0x05:
						//timer.TIMA = value;
						break;
					case 0x06:
						timer.TMA = value;
						break;
					case 0x07:
						timer.TAC = value;
						break;

						///Graphics
					case 0x40:
						gfx.LCDC = value;
						break;
					case 0x41:
						gfx.STAT = value;
						break;
					case 0x42:
						gfx.SCY = value;
						break;
					case 0x43:
						gfx.SCX = value;
						break;
					case 0x45:
						gfx.LYC = value;
						break;
					case 0x47:
						gfx.BGP = value;
						break;
					case 0x48:
						gfx.OBP0 = value;
						break;
					case 0x49:
						gfx.OBP1 = value;
						break;
					case 0x4A:
						gfx.WY = value;
						break;
					case 0x4B:
						gfx.WX = value;
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

		public MemoryMap(IMemoryBankController cart, GraphicsController gfx, InterruptScheduler timer)
		{
			internalRam = new byte[0x2000];
			highRam = new byte[0x80];
			this.cart = cart;
			this.gfx = gfx;
			this.timer = timer;
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

