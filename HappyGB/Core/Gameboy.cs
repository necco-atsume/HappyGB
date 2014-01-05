using System;

namespace HappyGB.Core
{
	public class Gameboy
	{
		IMemoryBankController cart;
		GraphicsController gfx;
		MemoryMap mem;
		InterruptScheduler timer;
		GBZ80 cpu;

		public Gameboy()
		{
			CartReader cartReader = new CartReader("Tetris.gb");
			cart = cartReader.CreateMBC();
			cartReader.Dispose();
			gfx = new GraphicsController();
			timer = new InterruptScheduler();
			mem = new MemoryMap(cart, gfx, timer);
			cpu = new GBZ80(mem);
		}

		public void Initialize()
		{
			cpu.Reset();
			//Other init code here. Set graphics/io to default states.
		}

		public void RunOneFrame()
		{
			cpu.Run(gfx, timer);
		}

		public ISurface GetSurface() //FIXME: Should be property.
		{
			return gfx.Surface;
		}
	}
}

