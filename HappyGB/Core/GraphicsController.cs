using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HappyGB.Core
{
	public class GraphicsController
	{
		private bool lcdcVblank, lcdcHblank, lcdcOAM, lcdcScanline;
		//fixme: raise the vblank interrupt in here.
		private const int SCANLINE_INTERVAL = 456;
		private byte[] vram;
		private byte[] oam;

		private int scanline;
		private int clock;
		public enum LCDState
		{
			VBlank,
			HBlank,
			OamAccess,
			LCDCopy
		}

		private LCDState state;

		private GbPalette obp0, obp1, bg;

		private ISurface buffer;
		private SurfaceBlitter blitter;

		public ISurface Surface
		{
			get { return buffer; }
		}

		//TODO: Registers.
		public byte LCDC
		{
			get;
			set;
		}

		public byte STAT 
		{
			get;
			set;
		}

		public byte SCY 
		{
			get;
			set;
		}

		public byte SCX
		{
			get;
			set;
		}

		public byte LY
		{
			get { return (byte)scanline; }
			set { scanline = 0; } //FIXME: Should this actually do that? That'll f things up! 
		}

		public byte LYC
		{
			get;
			set;
		}

		//Palettes: Update the SurfaceBlitter.
		public byte BGP 
		{
			get { return bg.RegisterValue; }
			set { bg.RegisterValue = value; }
		}

		public byte OBP0 
		{
			get { return obp0.RegisterValue; }
			set { obp0.RegisterValue = value; }

		}

		public byte OBP1 
		{
			get { return obp1.RegisterValue; }
			set { obp1.RegisterValue = value; }
		}

		public byte WY 
		{
			get;
			set;
		}

		public byte WX 
		{
			get;
			set;
		}

		public GraphicsController()
		{
			buffer = new ScreenSurface();
			state = LCDState.HBlank;
			clock = scanline = 0;
			lcdcHblank = lcdcOAM = lcdcScanline = lcdcVblank = false;
			blitter = new SurfaceBlitter();
			vram = new byte[0x2000];
			oam = new byte[0xA0];

			bg = new GbPalette(false);
			obp0 = new GbPalette(true);
			obp1 = new GbPalette(true);
		}

		/// <summary>
		/// Updates the graphics controller with the given clock.
		/// </summary>
		public InterruptType Update(int ticks)
		{
			clock += ticks;
			switch (state) 
			{
			case LCDState.HBlank:
				if (clock > 201) 
				{
					clock = 0;
					state = LCDState.OamAccess;
				}
				break;

			case LCDState.VBlank:
				//We update the scanline here too.
				scanline = (144 + (clock % SCANLINE_INTERVAL));
				if (clock == ticks) //since we just booped the counter.
				{
					if (lcdcVblank) //FIXME: we can't have two interupts @ the same time right?
						return InterruptType.VBlank | InterruptType.LCDController;
					else return InterruptType.VBlank; //This should work???
				}
				if (clock > 4560) 
				{
					clock = 0;
					state = LCDState.OamAccess;
					if (lcdcOAM)
						return InterruptType.LCDController;
				}
				break;

			case LCDState.OamAccess:
				if (clock > 77) {
					clock = 0;
					state = LCDState.LCDCopy;
				}
				break;
			case LCDState.LCDCopy:
				if(clock > 169)
				{
					if (scanline >= 144)
					{
						state = LCDState.VBlank; 
						clock = 0;
					}
					else
					{
						clock = 0;
						WriteScanline();
						scanline++;
						state = LCDState.HBlank;
					}

					if (LY == LYC) {
						//TODO: If we have ly=lyc interrupts enabled fire one.
						return InterruptType.LCDController;
					}
				}

				break;
			}
			return InterruptType.None;
		}

		public byte ReadOAM8(ushort address)
		{
			return oam[address - 0xFE00];
		}

		public void WriteOAM8(ushort address, byte value)
		{
			oam[address - 0xFE00] = value;
		}

		public byte ReadVRAM8(ushort address)
		{
			return vram[address - 0x8000];
		}

		public void WriteVRAM8(ushort address, byte value)
		{
			vram[address - 0x8000] = value;
		}

		/// <summary>
		/// Writes one full scanline to the back buffer.
		/// </summary>
		private unsafe void WriteScanline()
		{
			for (int x = 0; x < 160; x += 8) 
			{
				ushort backAddrBase = ((LCDC & 0x80) == 0x80) ? (ushort)0x9C00 : (ushort)0x9800;
				ushort windAddrBase = ((LCDC & 0x40) == 0x40) ? (ushort)0x9C00 : (ushort)0x9800;
				ushort tileDataBase = ((LCDC & 0x10) == 0x10) ? (ushort)0x8000 : (ushort)0x8800;
				bool signedTileData = backAddrBase == 0x8000;

				bool windowEnabled = ((LCDC & 0x20) == 0x20);

				//get the tile at the x,y.
				//Do BG first.
				int bTileRow = ((SCY + scanline) & 0xFF) / 32;
				int bTileCol = ((SCX + x) & 0xFF) / 32;
				ushort tileOffset = (ushort) (backAddrBase + (bTileRow * 32) + bTileCol);

				ushort dataOffsetAddr = 0;
				if (!signedTileData)
				{
					byte tileNumber = ReadVRAM8(tileOffset);
					dataOffsetAddr = (ushort)(tileDataBase + tileNumber);
				}
				else
				{
					sbyte tileNumber = unchecked((sbyte)ReadVRAM8(tileOffset));
					dataOffsetAddr = (ushort)(tileDataBase + tileNumber);
				}

				//Add scanline to tile offset so we get the correct tile scanline.
				dataOffsetAddr += (ushort)(((scanline + SCY) % 8) * 2);

				byte tileHi = ReadVRAM8(dataOffsetAddr);
				byte tileLo = ReadVRAM8((ushort)(dataOffsetAddr + 1));

				//draw that tile to the buffer.
				DrawTileScan(bg, tileHi, tileLo, x - (SCX % 8), scanline);
				//Now draw window

				//now draw sprites.
			}
		}

		private unsafe void DrawTileScan(GbPalette pal, byte tileHigh, byte tileLow, int x, int y)
		{
			//Put both in the same int so we only have to shift once per thing.
			int bit = (tileHigh << 8) + tileLow;

			int absOffset = ((y - SCY) * 160) + x;

			//Draw 8 pixels.
			for (int b = 0; b < 8; b++)
			{
				//Don't draw off the screen.
				if (x + b < 0) continue;
				if (x + b > 160) continue;

				//Get the color from the tile's data.
				byte paletteColor = 0;
				if ((bit & 0x01) == 0x01)
					paletteColor |= 0x01;
				if ((bit & 0x10) == 0x10)
					paletteColor |= 0x02;

				Color c = pal.GetColor(paletteColor);

				//Draw to the thing.
				Surface.Buffer[absOffset] = c;
				absOffset++;

				bit = bit >> 2;
			}
		}
	}
}

