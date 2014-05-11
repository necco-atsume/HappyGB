using System;

namespace HappyGB.Core
{
    public class Gameboy
    {
        IMemoryBankController cart;
        MemoryMap mem;
        TimerController timer;
        GBZ80 cpu;

        IInputProvider input;

        //Hack: this is public for now
        public GraphicsController gfx
        {
            get;
            private set;
        }
        public Gameboy(IInputProvider input)
        {
            CartReader cartReader = new CartReader("02-write_timing.gb");
            cart = cartReader.CreateMBC();
            cartReader.Dispose();
            gfx = new GraphicsController();
            timer = new TimerController();
            mem = new MemoryMap(cart, gfx, timer, input);
            cpu = new GBZ80(mem);
            this.input = input;
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

