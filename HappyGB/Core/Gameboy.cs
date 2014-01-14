using System;

namespace HappyGB.Core
{
    public class Gameboy
    {
        IMemoryBankController cart;
        MemoryMap mem;
        TimerController timer;
        GBZ80 cpu;

        //Hack: this is public for now
        public GraphicsController gfx
        {
            get;
            private set;
        }
        public Gameboy()
        {
            CartReader cartReader = new CartReader("cpu_instrs.gb");
            cart = cartReader.CreateMBC();
            cartReader.Dispose();
            gfx = new GraphicsController();
            timer = new TimerController();
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

