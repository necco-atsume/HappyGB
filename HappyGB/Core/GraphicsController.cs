using System;

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
			get;
			set;
		}

		public byte OBP0 
		{
			get;
			set;
		}

		public byte OBP1 
		{
			get;
			set;
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
					//TODO: Fire off a VBlank interrupt.
					if (lcdcVblank)
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
					clock = 0;
					WriteScanline();
					scanline++;
					state = LCDState.HBlank;

					if (scanline == 144)
					{
						state = LCDState.VBlank;
						clock = 0;
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
			throw new NotImplementedException();
		}

		public void WriteOAM8(ushort address, byte value)
		{
			throw new NotImplementedException();
		}

		public byte ReadVRAM8(ushort address)
		{
			throw new NotImplementedException();
		}

		public void WriteVRAM8(ushort address, byte value)
		{
			//Update the surface blitter.
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes one full scanline to the back buffer.
		/// </summary>
		private void WriteScanline()
		{
			//Get x, y index in tile, and stuff, then draw the tiles.
			throw new NotImplementedException();
			//blitter.DrawTileScan( ...
		}
	}
}

