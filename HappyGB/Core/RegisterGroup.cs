using System;
using System.Runtime.InteropServices;

namespace HappyGB.Core
{
	/// <summary>
	/// The group of registers for a CPU.
	/// Some crazy union thing so it's way faster,
	/// and easy to implement af/bc without bitwise stuffs.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct RegisterGroup
	{
		[FieldOffset(0)]
		public ushort af;
		[FieldOffset(2)]
		public ushort bc;
		[FieldOffset(4)]
		public ushort de;
		[FieldOffset(6)]
		public ushort hl;

		[FieldOffset(8)]
		public ushort sp;
		[FieldOffset(10)]
		public ushort pc;

		[FieldOffset(1)]
		public byte a;
		[FieldOffset(0)]
		public byte _f; //Don't use me.
		[FieldOffset(3)]
		public byte b;
		[FieldOffset(2)]
		public byte c;
		[FieldOffset(5)]
		public byte d;
		[FieldOffset(4)]
		public byte e;
		[FieldOffset(7)]
		public byte h;
		[FieldOffset(6)]
		public byte l;

		[FieldOffset(12)]
		public bool ZF;
		[FieldOffset(13)]
		public bool CF;
		[FieldOffset(14)]
		public bool HCF;
		[FieldOffset(15)]
		public bool NF;

		//FieldOffset hacks for "immediate" and "indirect" pointer hacks.
		//This will let us make opcodes like
		//R.ind8 = M[R.hl];
		//tick(4);
		//ADD_a_n(*ind);
		//M[R.hl] = R.ind8;

		//TODO: GEt rid of this.
		[FieldOffset(16)]
		public ushort ind16;
		[FieldOffset(16)]
		public ushort imm16;

		[FieldOffset(16)]
		public byte ind8;
		[FieldOffset(16)]
		public byte imm8;

		public void UpdateFlagsFromAF()
		{
			ZF  = ((_f & 0x80) == 0x80);
			NF  = ((_f & 0x40) == 0x40);
			HCF = ((_f & 0x20) == 0x20);
			CF  = ((_f & 0x10) == 0x10);
		}

		public void UpdateAFFromFlags()
		{
			//_f &= 0x0F; //more accurate but unnecessary?
			_f = 0;
			if(ZF)
				_f |= 0x80;
			if(NF)
				_f |= 0x40;
			if(HCF)
				_f |= 0x20;
			if(CF)
				_f |= 0x10;
		}

		public override string ToString()
		{
			return string.Format("[a:{0:X2} b:{1:X2} c:{2:X2} d:{3:X2} e:{4:X2} h:{5:X2} l:{6:X2}] [pc:{7:X4} sp:{8:X4}] hl: {9:X4}] [{10}{11}{12}{13}]\n",
				a, b, c, d, e, h, l, pc, sp, hl,
                ZF? "Z": "-",
                NF? "N": "-",
                HCF? "H": "-",
                CF? "C": "-");
		}
	}
}

