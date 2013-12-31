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

		[FieldOffset(0)]
		public byte a;
		[FieldOffset(1)]
		public byte _f; //Don't use me.
		[FieldOffset(2)]
		public byte b;
		[FieldOffset(3)]
		public byte c;
		[FieldOffset(4)]
		public byte d;
		[FieldOffset(5)]
		public byte e;
		[FieldOffset(6)]
		public byte h;
		[FieldOffset(7)]
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
	}
}

