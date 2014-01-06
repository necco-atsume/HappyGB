using System;

namespace HappyGB.Core
{
	public partial class GBZ80
	{
		public unsafe void Execute()
		{
			fixed(RegisterGroup* rp = &R)
			{
				byte opFirst = Fetch8();
				//System.Diagnostics.Debug.WriteLine("op:" + opFirst.ToString("X"));

				switch (opFirst)
				{
				case 0x00:
					Tick(4);
					break;
				case 0x01:
					LD_rr_imm16(&rp->bc);
					Tick(12);
					break;
				case 0x02:
					M.Write16(R.bc, R.a);
					Tick(8);
					break;
				case 0x03:
					INC_nn(&rp->bc);
					Tick(8);
					break;
				case 0x04:
					INC_n(&rp->b);
					Tick(4);
					break;
				case 0x05:
					DEC_n(&rp->b);
					Tick(4);
					break;
				case 0x06:
					R.b = Fetch8();
					Tick(8);
					break;
				case 0x07:
					RLC(&rp->a);
					Tick(4);
					break;
				case 0x08:
					LD_imm16_rr(R.sp);
					Tick(20);
					break;
				case 0x09:
					ushort imm = M.Read16(R.bc);
					ADD_HL_rr(imm);
					Tick(8);
					break;
				case 0x0A:
					R.a = M[R.bc];
					Tick(8);
					break;
				case 0x0B:
					DEC_nn(&rp->bc);
					Tick(8);
					break;
				case 0x0C:
					INC_n(&rp->c);
					Tick(4);
					break;
				case 0x0D:
					DEC_n(&rp->c);
					Tick(4);
					break;
				case 0x0E:
					R.c = Fetch8();
					Tick(8);
					break;
				case 0x0F:
					RRC(&rp->a);
					Tick(4);
					break;
				case 0x10:
					STOP();
					Tick(4);
					break;
				case 0x11:
					LD_rr_imm16(&rp->de);
					Tick(12);
					break;
				case 0x12:
					M.Write16(R.de, R.a);
					Tick(8);
					break;
				case 0x13:
					INC_nn(&rp->de);
					Tick(8);
					break;
				case 0x14:
					INC_n(&rp->d);
					Tick(4);
					break;
				case 0x15:
					DEC_n(&rp->d);
					Tick(4);
					break;
				case 0x16:
					R.d = Fetch8();
					Tick(8);
					break;
				case 0x17:
					RL(&rp->a);
					Tick(4);
					break;
				case 0x18:
					JP_relative(true);
					Tick(8);
					break;
				case 0x19:
					ADD_HL_rr(R.de);
					Tick(8);
					break;
				case 0x1A:
					R.a = M[R.de];
					Tick(8);
					break;
				case 0x1B:
					DEC_nn(&rp->de);
					Tick(8);
					break;
				case 0x1C:
					INC_n(&rp->e);
					Tick(4);
					break;
				case 0x1D:
					DEC_n(&rp->e);
					Tick(4);
					break;
				case 0x1E:
					R.e = Fetch8();
					Tick(8);
					break;
				case 0x1F:
					RR(&rp->a);
					Tick(4);
					break;
				case 0x20:
					JP_relative(!R.ZF);
					Tick(8); 
					break;
				case 0x21:
					R.hl = Fetch16();
					Tick(12);
					break;
				case 0x22:
					M.Write16(R.hl, R.a);
					R.hl++;
					Tick(8);
					break;
				case 0x23:
					INC_nn(&rp->hl);
					Tick(8);
					break;
				case 0x24:
					INC_n(&rp->h);
					Tick(4);
					break;
				case 0x25:
					DEC_n(&rp->h);
					Tick(4);
					break;
				case 0x26:
					R.h = Fetch8();
					Tick(8);
					break;
				case 0x27:
					DAA();
					Tick(4);
					break;
				case 0x28:
					JP_relative(R.ZF);
					Tick(8);
					break;
				case 0x29:
					ADD_HL_rr(R.hl);
					Tick(8);
					break;
				case 0x2A:
					R.a = M[R.hl];
					R.hl++;
					Tick(8);
					break;
				case 0x2B:
					DEC_nn(&rp->hl);
					Tick(8);
					break;
				case 0x2C:
					INC_n(&rp->l);
					Tick(4);
					break;
				case 0x2D:
					DEC_n(&rp->l);
					Tick(4);
					break;
				case 0x2E:
					R.l = Fetch8();
					Tick(8);
					break;
				case 0x2F:
					CPL();
					Tick(4);
					break;
				case 0x30:
					JP_relative(!R.CF);
					Tick(8);
					break;
				case 0x31:
					R.sp = Fetch16();
					Tick(12);
					break;
				case 0x32:
					M.Write16(R.hl, R.a);
					R.hl--;
					Tick(8);
					break;
				case 0x33:
					INC_nn(&rp->sp);
					Tick(8);
					break;
				case 0x34:
					R.ind8 = M[R.hl];
					INC_n(&rp->ind8);
					M[R.hl] = R.ind8;
					Tick(12);
					break;
				case 0x35:
					R.ind8 = M[R.hl];
					DEC_n(&rp->ind8);
					M[R.hl] = R.ind8;
					Tick(12);
					break;
				case 0x36:
					M[R.hl] = Fetch8();
					Tick(12);
					break;
				case 0x37:
					SCF();
					Tick(4);
					break;
				case 0x38:
					JP_relative(R.CF);
					Tick(8);
					break;
				case 0x39:
					ADD_HL_rr(R.sp);
					Tick(8);
					break;
				case 0x3A:
					R.a = M[R.hl];
					R.hl--;
					Tick(8);
					break;
				case 0x3B:
					R.sp--;
					Tick(8);
					break;
				case 0x3C:
					INC_n(&rp->a);
					Tick(4);
					break;
				case 0x3D:
					DEC_n(&rp->a);
					Tick(4);
					break;
				case 0x3E:
					R.a = Fetch8();
					Tick(8);
					break;
				case 0x3F:
					CCF();
					Tick(4);
					break;
				case 0x40:
					//LD b,b
					Tick(4);
					break;
				case 0x41:
					R.b = R.c;
					Tick(4);
					break;
				case 0x42:
					R.b = R.d;
					Tick(4);
					break;
				case 0x43:
					R.b = R.e;
					Tick(4);
					break;
				case 0x44:
					R.b = R.h;
					Tick(4);
					break;
				case 0x45:
					R.b = R.l;
					Tick(4);
					break;
				case 0x46:
					R.b = M[R.hl];
					Tick(8);
					break;
				case 0x47:
					R.b = R.a;
					Tick(4);
					break;
				case 0x48:
					R.c = R.b;
					Tick(4);
					break;
				case 0x49:
					//LD c, c
					Tick(4);
					break;
				case 0x4A:
					R.c = R.d;
					Tick(4);
					break;
				case 0x4B:
					R.c = R.e;
					Tick(4);
					break;
				case 0x4C:
					R.c = R.h;
					Tick(4);
					break;
				case 0x4D:
					R.c = R.l;
					Tick(4);
					break;
				case 0x4E:
					R.c = M[R.hl];
					Tick(8);
					break;
				case 0x4F:
					R.c = R.a;
					Tick(4);
					break;
				case 0x50:
					R.d = R.b;
					Tick(4);
					break;
				case 0x51:
					R.d = R.c;
					Tick(4);
					break;
				case 0x52:
					Tick(4);
					break;
				case 0x53:
					R.d = R.e;
					Tick(4);
					break;
				case 0x54:
					R.d = R.h;
					Tick(4);
					break;
				case 0x55:
					R.d = R.l;
					Tick(4);
					break;
				case 0x56:
					R.d = M[R.hl]; 
					Tick(8);
					break;
				case 0x57:
					R.d = R.a;
					Tick(4);
					break;
				case 0x58:
					R.e = R.b;
					Tick(4);
					break;
				case 0x59:
					R.e = R.c;
					Tick(4);
					break;
				case 0x5A:
					R.e = R.d;
					Tick(4);
					break;
				case 0x5B:
					Tick(4);
					break;
				case 0x5C:
					R.e = R.h;
					Tick(4);
					break;
				case 0x5D:
					R.e = R.l;
					Tick(4);
					break;
				case 0x5E:
					R.e = M[R.hl];
					Tick(8);
					break;
				case 0x5F:
					R.e = R.a;
					Tick(4);
					break;
				case 0x60:
					R.h = R.b;
					Tick(4);
					break;
				case 0x61:
					R.h = R.c;
					Tick(4);
					break;
				case 0x62:
					R.h = R.d;
					Tick(4);
					break;
				case 0x63:
					R.h = R.e;
					Tick(4);
					break;
				case 0x64:
					R.h = R.h;
					Tick(4);
					break;
				case 0x65:
					R.h = R.l;
					Tick(4);
					break;
				case 0x66:
					R.h = M[R.hl];
					Tick(8);
					break;
				case 0x67:
					R.h = R.a;
					Tick(4);
					break;
				case 0x68:
					R.l = R.b;
					Tick(4);
					break;
				case 0x69:
					R.l = R.c;
					Tick(4);
					break;
				case 0x6A:
					R.l = R.d;
					Tick(4);
					break;
				case 0x6B:
					R.l = R.e;
					Tick(4);
					break;
				case 0x6C:
					R.l = R.h;
					Tick(4);
					break;
				case 0x6D:
					R.l = R.l;
					Tick(4);
					break;
				case 0x6E:
					R.l = M[R.hl];
					Tick(4);
					break;
				case 0x6F:
					R.l = R.a;
					Tick(4);
					break;
				case 0x70:
					M[R.hl] = R.b;
					Tick(8);
					break;
				case 0x71:
					M[R.hl] = R.c;
					Tick(8);
					break;
				case 0x72:
					M[R.hl] = R.d;
					Tick(8);
					break;
				case 0x73:
					M[R.hl] = R.e;
					Tick(8);
					break;
				case 0x74:
					M[R.hl] = R.h;
					Tick(8);
					break;
				case 0x75:
					M[R.hl] = R.l;
					Tick(8);
					break;
				case 0x76:
					HLT();
					Tick(4);
					break;
				case 0x77:
					M[R.hl] = R.a;
					Tick(8);
					break;
				case 0x78:
					R.a = R.b;
					Tick(4);
					break;
				case 0x79:
					R.a = R.c;
					Tick(4);
					break;
				case 0x7A:
					R.a = R.d;
					Tick(4);
					break;
				case 0x7B:
					R.a = R.e;
					Tick(4);
					break;
				case 0x7C:
					R.a = R.h;
					Tick(4);
					break;
				case 0x7D:
					R.a = R.l;
					Tick(4);
					break;
				case 0x7E:
					R.a = M[R.hl];
					Tick(4);
					break;
				case 0x7F:
					R.a = R.a;
					Tick(4);
					break;
				case 0x80:
					ADD_A_n(R.b);
					Tick(4);
					break;
				case 0x81:
					ADD_A_n(R.c);
					Tick(4);
					break;
				case 0x82:
					ADD_A_n(R.d);
					Tick(4);
					break;
				case 0x83:
					ADD_A_n(R.e);
					Tick(4);
					break;
				case 0x84:
					ADD_A_n(R.h);
					Tick(4);
					break;
				case 0x85:
					ADD_A_n(R.l);
					Tick(4);
					break;
				case 0x86:
					ADD_A_n(M[R.hl]);
					Tick(8);
					break;
				case 0x87:
					ADD_A_n(R.a);
					Tick(4);
					break;
				case 0x88:
					ADC_A_n(R.b);
					Tick(4);
					break;
				case 0x89:
					ADC_A_n(R.c);
					Tick(4);
					break;
				case 0x8A:
					ADC_A_n(R.d);
					Tick(4);
					break;
				case 0x8B:
					ADC_A_n(R.e);
					Tick(4);
					break;
				case 0x8C:
					ADC_A_n(R.h);
					Tick(4);
					break;
				case 0x8D:
					ADC_A_n(R.l);
					Tick(4);
					break;
				case 0x8E:
					ADC_A_n(M[R.hl]);
					Tick(4);
					break;
				case 0x8F:
					ADC_A_n(R.a);
					Tick(4);
					break;
				case 0x90:
					SUB_A_n(R.b);
					Tick(4);
					break;
				case 0x91:
					SUB_A_n(R.c);
					Tick(4);
					break;
				case 0x92:
					SUB_A_n(R.d);
					Tick(4);
					break;
				case 0x93:
					SUB_A_n(R.e);
					Tick(4);
					break;
				case 0x94:
					SUB_A_n(R.h);
					Tick(4);
					break;
				case 0x95:
					SUB_A_n(R.l);
					Tick(4);
					break;
				case 0x96:
					SUB_A_n(M[R.hl]);
					Tick(8);
					break;
				case 0x97:
					SUB_A_n(R.a);
					Tick(4);
					break;
				case 0x98:
					SBC_A_n(R.b);
					Tick(4);
					break;
				case 0x99:
					SBC_A_n(R.c);
					Tick(4);
					break;
				case 0x9A:
					SBC_A_n(R.d);
					Tick(4);
					break;
				case 0x9B:
					SBC_A_n(R.e);
					Tick(4);
					break;
				case 0x9C:
					SBC_A_n(R.h);
					Tick(4);
					break;
				case 0x9D:
					SBC_A_n(R.l);
					Tick(4);
					break;
				case 0x9E:
					SBC_A_n(M[R.hl]);
					Tick(4);
					break;
				case 0x9F:
					SBC_A_n(R.a);
					Tick(4);
					break;
				case 0xA0:
					AND_A_n(R.b);
					Tick(4);
					break;
				case 0xA1:
					AND_A_n(R.c);
					Tick(4);
					break;
				case 0xA2:
					AND_A_n(R.d);
					Tick(4);
					break;
				case 0xA3:
					AND_A_n(R.e);
					Tick(4);
					break;
				case 0xA4:
					AND_A_n(R.h);
					Tick(4);
					break;
				case 0xA5:
					AND_A_n(R.l);
					Tick(4);
					break;
				case 0xA6:
					AND_A_n(M[R.hl]);
					Tick(8);
					break;
				case 0xA7:
					AND_A_n(R.a);
					Tick(4);
					break;
				case 0xA8:
					XOR_A_n(R.b);
					Tick(4);
					break;
				case 0xA9:
					XOR_A_n(R.c);
					Tick(4);
					break;
				case 0xAA:
					XOR_A_n(R.d);
					Tick(4);
					break;
				case 0xAB:
					XOR_A_n(R.e);
					Tick(4);
					break;
				case 0xAC:
					XOR_A_n(R.h);
					Tick(4);
					break;
				case 0xAD:
					XOR_A_n(R.l);
					Tick(4);
					break;
				case 0xAE:
					XOR_A_n(M[R.hl]);
					Tick(4);
					break;
				case 0xAF:
					XOR_A_n(R.a);
					Tick(4);
					break;
				case 0xB0:
					OR_A_n(R.b);
					Tick(4);
					break;
				case 0xB1:
					OR_A_n(R.c);
					Tick(4);
					break;
				case 0xB2:
					OR_A_n(R.d);
					Tick(4);
					break;
				case 0xB3:
					OR_A_n(R.e);
					Tick(4);
					break;
				case 0xB4:
					OR_A_n(R.h);
					Tick(4);
					break;
				case 0xB5:
					OR_A_n(R.l);
					Tick(4);
					break;
				case 0xB6:
					OR_A_n(M[R.hl]);
					Tick(8);
					break;
				case 0xB7:
					OR_A_n(R.a);
					Tick(4);
					break;
				case 0xB8:
					CP_A_n(R.b);
					Tick(4);
					break;
				case 0xB9:
					CP_A_n(R.c);
					Tick(4);
					break;
				case 0xBA:
					CP_A_n(R.d);
					Tick(4);
					break;
				case 0xBB:
					CP_A_n(R.e);
					Tick(4);
					break;
				case 0xBC:
					CP_A_n(R.h);
					Tick(4);
					break;
				case 0xBD:
					CP_A_n(R.l);
					Tick(4);
					break;
				case 0xBE:
					CP_A_n(M[R.hl]);
					Tick(4);
					break;
				case 0xBF:
					CP_A_n(R.a);
					Tick(4);
					break;
				case 0xC0:
					RET_cond(!R.ZF);
					Tick(8);
					break;
				case 0xC1:
					POP(&rp->bc);
					Tick(12);
					break;
				case 0xC2:
					JP(Fetch16(), !R.ZF);
					Tick(12);
					break;
				case 0xC3:
					JP(Fetch16(), true);
					Tick(12);
					break;
				case 0xC4:
					CALL(!R.ZF);
					Tick(12);
					break;
				case 0xC5:
					PUSH(R.bc);
					Tick(16);
					break;
				case 0xC6:
					ADD_A_n(Fetch8());
					Tick(8);
					break;
				case 0xC7:
					RST(0x00);
					Tick(16);
					break;
				case 0xC8:
					RET_cond(R.ZF);
					Tick(8);
					break;
				case 0xC9:
					RET();
					Tick(16);
					break;
				case 0xCA:
					JP(Fetch16(), R.ZF);
					Tick(12);
					break;
				case 0xCB:
					CBExecute();
					break;
				case 0xCC:
					CALL(R.ZF);
					Tick(12);
					break;
				case 0xCD:
					CALL(true);
					Tick(12);
					break;
				case 0xCE:
					ADC_A_n(Fetch8());
					Tick(8);
					break;
				case 0xCF:
					RST(0x08);
					Tick(16);
					break;
				case 0xD0:
					RET_cond(!R.CF);
					Tick(8);
					break;
				case 0xD1:
					POP(&rp->de);
					Tick(12);
					break;
				case 0xD2:
					JP(Fetch16(), !R.CF);
					Tick(12);
					break;
				case 0xD4:
					CALL(!R.CF);
					Tick(12);
					break;
				case 0xD5:
					PUSH(R.de);
					Tick(16);
					break;
				case 0xD6:
					SUB_A_n(Fetch8());
					Tick(8);
					break;
				case 0xD7:
					RST(0x10);
					Tick(16);
					break;
				case 0xD8:
					RET_cond(R.CF);
					Tick(8);
					break;
				case 0xD9:
					RET();
					cpuInterruptEnable = true;
					Tick(16);
					break;
				case 0xDA:
					JP(Fetch16(), R.CF);
					Tick(12);
					break;
				case 0xDC:
					CALL(R.CF);
					Tick(12);
					break;
				case 0xDE:
					SBC_A_n(Fetch8());
					Tick(8);
					break;
				case 0xDF:
					RST(0x18);
					Tick(16);
					break;
				case 0xE0:
					M[(ushort)(Fetch8() + 0xFF00)] = R.a;
					Tick(12);
					break;
				case 0xE1:
					POP(&rp->hl);
					Tick(12);
					break;
				case 0xE2:
					M[(ushort)(R.c + 0xFF00)] = R.a;
					Tick(8);
					break;
				case 0xE5:
					PUSH(R.hl);
					Tick(16);
					break;
				case 0xE6:
					AND_A_n(Fetch8());
					Tick(8);
					break;
				case 0xE7:
					RST(0x20);
					Tick(16);
					break;
				case 0xE8:
					ADD_SP_n(Fetch8());
					Tick(16);
					break;
				case 0xE9:
					JP(M[R.hl], true);
					//no tick; ticks 4 in jp.
					//FIXME: 8bit data?
					break;
				case 0xEA:
					M[Fetch16()] = R.a;
					Tick(16);
					break;
				case 0xEE:
					XOR_A_n(Fetch8());
					Tick(8);
					break;
				case 0xEF:
					RST(0x28);
					Tick(16);
					break;
				case 0xF0:
					R.a = M[(ushort)(0xFF00 + Fetch8())];
					Tick(12);
					break;
				case 0xF1:
					POP(&rp->af);
					R.UpdateFlagsFromAF();
					Tick(12);
					break;
				case 0xF2:
					R.a = M[(ushort)(R.c + 0xFF00)];
					Tick(8);
					break;
				case 0xF3:
					cpuInterruptEnable = false;
					Tick(4);
					break;
				case 0xF5:
					R.UpdateAFFromFlags();
					PUSH(R.af);
					Tick(16);
					break;
				case 0xF6:
					OR_A_n(Fetch8());
					Tick(8);
					break;
				case 0xF7:
					RST(0x30);
					Tick(16);
					break;
				case 0xF8:
					LD_hl_sp_imm();
					Tick(12);
					break;
				case 0xF9:
					R.hl = R.sp;
					Tick(8);
					break;
				case 0xFA:
					R.a = M[Fetch16()];
					Tick(16);
					break;
				case 0xFB:
					cpuInterruptEnable = true;
					Tick(4);
					break;
				case 0xFE:
					CP_A_n(Fetch8());
					Tick(8);
					break;
				case 0xFF:
					RST(0x38);
					Tick(16);
					break;
				default:
					throw new Exception("invalid opcode 0x" + opFirst.ToString("x"));
				} 
			}
		}

		public void CBExecute()
		{
			byte opCB = Fetch8();

			throw new NotImplementedException();
			switch (opCB) 
			{
			case 0x00:
				break;
			case 0x01:
				break;
			case 0x02:
				break;
			case 0x03:
				break;
			case 0x04:
				break;
			case 0x05:
				break;
			case 0x06:
				break;
			case 0x07:
				break;
			case 0x08:
				break;
			case 0x09:
				break;
			case 0x0A:
				break;
			case 0x0B:
				break;
			case 0x0C:
				break;
			case 0x0D:
				break;
			case 0x0E:
				break;
			case 0x0F:
				break;
			case 0x10:
				break;
			case 0x11:
				break;
			case 0x12:
				break;
			case 0x13:
				break;
			case 0x14:
				break;
			case 0x15:
				break;
			case 0x16:
				break;
			case 0x17:
				break;
			case 0x18:
				break;
			case 0x19:
				break;
			case 0x1A:
				break;
			case 0x1B:
				break;
			case 0x1C:
				break;
			case 0x1D:
				break;
			case 0x1E:
				break;
			case 0x1F:
				break;
			case 0x20:
				break;
			case 0x21:
				break;
			case 0x22:
				break;
			case 0x23:
				break;
			case 0x24:
				break;
			case 0x25:
				break;
			case 0x26:
				break;
			case 0x27:
				break;
			case 0x28:
				break;
			case 0x29:
				break;
			case 0x2A:
				break;
			case 0x2B:
				break;
			case 0x2C:
				break;
			case 0x2D:
				break;
			case 0x2E:
				break;
			case 0x2F:
				break;
			case 0x30:
				break;
			case 0x31:
				break;
			case 0x32:
				break;
			case 0x33:
				break;
			case 0x34:
				break;
			case 0x35:
				break;
			case 0x36:
				break;
			case 0x37:
				break;
			case 0x38:
				break;
			case 0x39:
				break;
			case 0x3A:
				break;
			case 0x3B:
				break;
			case 0x3C:
				break;
			case 0x3D:
				break;
			case 0x3E:
				break;
			case 0x3F:
				break;
			case 0x40:
				break;
			case 0x41:
				break;
			case 0x42:
				break;
			case 0x43:
				break;
			case 0x44:
				break;
			case 0x45:
				break;
			case 0x46:
				break;
			case 0x47:
				break;
			case 0x48:
				break;
			case 0x49:
				break;
			case 0x4A:
				break;
			case 0x4B:
				break;
			case 0x4C:
				break;
			case 0x4D:
				break;
			case 0x4E:
				break;
			case 0x4F:
				break;
			case 0x50:
				break;
			case 0x51:
				break;
			case 0x52:
				break;
			case 0x53:
				break;
			case 0x54:
				break;
			case 0x55:
				break;
			case 0x56:
				break;
			case 0x57:
				break;
			case 0x58:
				break;
			case 0x59:
				break;
			case 0x5A:
				break;
			case 0x5B:
				break;
			case 0x5C:
				break;
			case 0x5D:
				break;
			case 0x5E:
				break;
			case 0x5F:
				break;
			case 0x60:
				break;
			case 0x61:
				break;
			case 0x62:
				break;
			case 0x63:
				break;
			case 0x64:
				break;
			case 0x65:
				break;
			case 0x66:
				break;
			case 0x67:
				break;
			case 0x68:
				break;
			case 0x69:
				break;
			case 0x6A:
				break;
			case 0x6B:
				break;
			case 0x6C:
				break;
			case 0x6D:
				break;
			case 0x6E:
				break;
			case 0x6F:
				break;
			case 0x70:
				break;
			case 0x71:
				break;
			case 0x72:
				break;
			case 0x73:
				break;
			case 0x74:
				break;
			case 0x75:
				break;
			case 0x76:
				break;
			case 0x77:
				break;
			case 0x78:
				break;
			case 0x79:
				break;
			case 0x7A:
				break;
			case 0x7B:
				break;
			case 0x7C:
				break;
			case 0x7D:
				break;
			case 0x7E:
				break;
			case 0x7F:
				break;
			case 0x80:
				break;
			case 0x81:
				break;
			case 0x82:
				break;
			case 0x83:
				break;
			case 0x84:
				break;
			case 0x85:
				break;
			case 0x86:
				break;
			case 0x87:
				break;
			case 0x88:
				break;
			case 0x89:
				break;
			case 0x8A:
				break;
			case 0x8B:
				break;
			case 0x8C:
				break;
			case 0x8D:
				break;
			case 0x8E:
				break;
			case 0x8F:
				break;
			case 0x90:
				break;
			case 0x91:
				break;
			case 0x92:
				break;
			case 0x93:
				break;
			case 0x94:
				break;
			case 0x95:
				break;
			case 0x96:
				break;
			case 0x97:
				break;
			case 0x98:
				break;
			case 0x99:
				break;
			case 0x9A:
				break;
			case 0x9B:
				break;
			case 0x9C:
				break;
			case 0x9D:
				break;
			case 0x9E:
				break;
			case 0x9F:
				break;
			case 0xA0:
				break;
			case 0xA1:
				break;
			case 0xA2:
				break;
			case 0xA3:
				break;
			case 0xA4:
				break;
			case 0xA5:
				break;
			case 0xA6:
				break;
			case 0xA7:
				break;
			case 0xA8:
				break;
			case 0xA9:
				break;
			case 0xAA:
				break;
			case 0xAB:
				break;
			case 0xAC:
				break;
			case 0xAD:
				break;
			case 0xAE:
				break;
			case 0xAF:
				break;
			case 0xB0:
				break;
			case 0xB1:
				break;
			case 0xB2:
				break;
			case 0xB3:
				break;
			case 0xB4:
				break;
			case 0xB5:
				break;
			case 0xB6:
				break;
			case 0xB7:
				break;
			case 0xB8:
				break;
			case 0xB9:
				break;
			case 0xBA:
				break;
			case 0xBB:
				break;
			case 0xBC:
				break;
			case 0xBD:
				break;
			case 0xBE:
				break;
			case 0xBF:
				break;
			case 0xC0:
				break;
			case 0xC1:
				break;
			case 0xC2:
				break;
			case 0xC3:
				break;
			case 0xC4:
				break;
			case 0xC5:
				break;
			case 0xC6:
				break;
			case 0xC7:
				break;
			case 0xC8:
				break;
			case 0xC9:
				break;
			case 0xCA:
				break;
			case 0xCB:
				break;
			case 0xCC:
				break;
			case 0xCD:
				break;
			case 0xCE:
				break;
			case 0xCF:
				break;
			case 0xD0:
				break;
			case 0xD1:
				break;
			case 0xD2:
				break;
			case 0xD3:
				break;
			case 0xD4:
				break;
			case 0xD5:
				break;
			case 0xD6:
				break;
			case 0xD7:
				break;
			case 0xD8:
				break;
			case 0xD9:
				break;
			case 0xDA:
				break;
			case 0xDB:
				break;
			case 0xDC:
				break;
			case 0xDD:
				break;
			case 0xDE:
				break;
			case 0xDF:
				break;
			case 0xE0:
				break;
			case 0xE1:
				break;
			case 0xE2:
				break;
			case 0xE3:
				break;
			case 0xE4:
				break;
			case 0xE5:
				break;
			case 0xE6:
				break;
			case 0xE7:
				break;
			case 0xE8:
				break;
			case 0xE9:
				break;
			case 0xEA:
				break;
			case 0xEB:
				break;
			case 0xEC:
				break;
			case 0xED:
				break;
			case 0xEE:
				break;
			case 0xEF:
				break;
			case 0xF0:
				break;
			case 0xF1:
				break;
			case 0xF2:
				break;
			case 0xF3:
				break;
			case 0xF4:
				break;
			case 0xF5:
				break;
			case 0xF6:
				break;
			case 0xF7:
				break;
			case 0xF8:
				break;
			case 0xF9:
				break;
			case 0xFA:
				break;
			case 0xFB:
				break;
			case 0xFC:
				break;
			case 0xFD:
				break;
			case 0xFE:
				break;
			case 0xFF:
				break;
			default:
				throw new Exception("Unsupported CB opcode " + opCB);
			}
		}
	}
}

