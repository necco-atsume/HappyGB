using System;

namespace HappyGB.Core
{
	public class Gameboy
	{
		IMemoryBankController cart;
		GraphicsController gfx;
		MemoryMap mem;
		IOController io;
		InterruptScheduler timer;
		GBZ80 cpu;

		public Gameboy()
		{
			CartReader cartReader = new CartReader("Tetris.gb");
			cart = cartReader.CreateMBC();
			cartReader.Dispose();
			io = new IOController();
			gfx = new GraphicsController();
			mem = new MemoryMap(cart, gfx, io);
			timer = new InterruptScheduler();
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

		public ISurface GetSurface()
		{
			return gfx.Surface;
		}
	}
}

