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

                //if (R.pc > 0xFF00)
                
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
                    M.Write8(R.bc, R.a);
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
                    M.Write8(R.de, R.a);
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
                    M.Write8(R.hl, R.a);
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
                    M.Write8(R.hl, R.a);
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
                    //LD b,b breakpoint.
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
                    //R.h = R.h;
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
                    //R.l = R.l;
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
                    //R.a = R.a;
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
                    //JP(M[R.hl], true);
                    JP(R.hl, true);
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
                    byte f = Fetch8();
                    R.a = M[(ushort)(0xFF00 + f)];
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

        public unsafe void CBExecute()
        {
            byte opCB = Fetch8();

            fixed(RegisterGroup* rp = &R)
            {
                switch (opCB) 
                {
                case 0x00:
                    RLC(&rp->b);
                    Tick(8);
                    break;
                case 0x01:
                    RLC(&rp->c);
                    Tick(8);
                    break;
                case 0x02:
                    RLC(&rp->d);
                    Tick(8);
                    break;
                case 0x03:
                    RLC(&rp->e);
                    Tick(8);
                    break;
                case 0x04:
                    RLC(&rp->h);
                    Tick(8);
                    break;
                case 0x05:
                    RLC(&rp->l);
                    Tick(8);
                    break;
                case 0x06:
                    rp->imm8 = M[R.hl];
                    RLC(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x07:
                    RLC(&rp->a);
                    Tick(8);
                    break;
                case 0x08:
                    RRC(&rp->b);
                    Tick(8);
                    break;
                case 0x09:
                    RRC(&rp->c);
                    Tick(8);
                    break;
                case 0x0A:
                    RRC(&rp->d);
                    Tick(8);
                    break;
                case 0x0B:
                    RRC(&rp->e);
                    Tick(8);
                    break;
                case 0x0C:
                    RRC(&rp->h);
                    Tick(8);
                    break;
                case 0x0D:
                    RRC(&rp->l);
                    Tick(8);
                    break;
                case 0x0E:
                    rp->imm8 = M[R.hl];
                    RRC(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x0F:
                    RRC(&rp->a);
                    Tick(8);
                    break;
                case 0x10:
                    RL(&rp->b);
                    Tick(8);
                    break;
                case 0x11:
                    RL(&rp->c);
                    Tick(8);
                    break;
                case 0x12:
                    RL(&rp->d);
                    Tick(8);
                    break;
                case 0x13:
                    RL(&rp->e);
                    Tick(8);
                    break;
                case 0x14:
                    RL(&rp->h);
                    Tick(8);
                    break;
                case 0x15:
                    RL(&rp->l);
                    Tick(8);
                    break;
                case 0x16:
                    rp->imm8 = M[R.hl];
                    RL(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x17:
                    RL(&rp->a);
                    Tick(8);
                    break;
                case 0x18:
                    RR(&rp->b);
                    Tick(8);
                    break;
                case 0x19:
                    RR(&rp->c);
                    Tick(8);
                    break;
                case 0x1A:
                    RR(&rp->d);
                    Tick(8);
                    break;
                case 0x1B:
                    RR(&rp->e);
                    Tick(8);
                    break;
                case 0x1C:
                    RR(&rp->h);
                    Tick(8);
                    break;
                case 0x1D:
                    RR(&rp->l);
                    Tick(8);
                    break;
                case 0x1E:
                    rp->imm8 = M[R.hl];
                    RR(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x1F:
                    RR(&rp->a);
                    Tick(8);
                    break;
                case 0x20:
                    SLA(&rp->b);
                    Tick(8);
                    break;
                case 0x21:
                    SLA(&rp->c);
                    Tick(8);
                    break;
                case 0x22:
                    SLA(&rp->d);
                    Tick(8);
                    break;
                case 0x23:
                    SLA(&rp->e);
                    Tick(8);
                    break;
                case 0x24:
                    SLA(&rp->h);
                    Tick(8);
                    break;
                case 0x25:
                    SLA(&rp->l);
                    Tick(8);
                    break;
                case 0x26:
                    rp->imm8 = M[R.hl];
                    SLA(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x27:
                    SLA(&rp->a);
                    Tick(8);
                    break;
                case 0x28:
                    SRA(&rp->b);
                    Tick(8);
                    break;
                case 0x29:
                    SRA(&rp->c);
                    Tick(8);
                    break;
                case 0x2A:
                    SRA(&rp->d);
                    Tick(8);
                    break;
                case 0x2B:
                    SRA(&rp->e);
                    Tick(8);
                    break;
                case 0x2C:
                    SRA(&rp->h);
                    Tick(8);
                    break;
                case 0x2D:
                    SRA(&rp->l);
                    Tick(8);
                    break;
                case 0x2E:
                    rp->imm8 = M[R.hl];
                    SRA(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x2F:
                    SRA(&rp->a);
                    Tick(8);
                    break;
                case 0x30:
                    SWAP(&rp->b);
                    Tick(8);
                    break;
                case 0x31:
                    SWAP(&rp->c);
                    Tick(8);
                    break;
                case 0x32:
                    SWAP(&rp->d);
                    Tick(8);
                    break;
                case 0x33:
                    SWAP(&rp->e);
                    Tick(8);
                    break;
                case 0x34:
                    SWAP(&rp->h);
                    Tick(8);
                    break;
                case 0x35:
                    SWAP(&rp->l);
                    Tick(8);
                    break;
                case 0x36:
                    rp->imm8 = M[R.hl];
                    SWAP(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x37:
                    SWAP(&rp->a);
                    Tick(8);
                    break;
                case 0x38:
                    SRL(&rp->b);
                    Tick(8);
                    break;
                case 0x39:
                    SRL(&rp->c);
                    Tick(8);
                    break;
                case 0x3A:
                    SRL(&rp->d);
                    Tick(8);
                    break;
                case 0x3B:
                    SRL(&rp->e);
                    Tick(8);
                    break;
                case 0x3C:
                    SRL(&rp->h);
                    Tick(8);
                    break;
                case 0x3D:
                    SRL(&rp->l);
                    Tick(8);
                    break;
                case 0x3E:
                    rp->imm8 = M[R.hl];
                    SRL(&rp->imm8);
                    M[R.hl] = rp->imm8;
                    Tick(16);
                    break;
                case 0x3F:
                    SRL(&rp->a);
                    Tick(8);
                    break;
                case 0x40:
                    BIT(R.b, 0x01);
                    Tick(8);
                    break;
                case 0x41:
                    BIT(R.c, 0x01);
                    Tick(8);
                    break;
                case 0x42:
                    BIT(R.d, 0x01);
                    Tick(8);
                    break;
                case 0x43:
                    BIT(R.e, 0x01);
                    Tick(8);
                    break;
                case 0x44:
                    BIT(R.h, 0x01);
                    Tick(8);
                    break;
                case 0x45:
                    BIT(R.l, 0x01);
                    Tick(8);
                    break;
                case 0x46:
                    BIT(M[R.hl], 0x01);
                    Tick(16);
                    break;
                case 0x47:
                    BIT(R.a, 0x01);
                    Tick(8);
                    break;
                case 0x48:
                    BIT(R.b, 0x02);
                    Tick(8);
                    break;
                case 0x49:
                    BIT(R.c, 0x02);
                    Tick(8);
                    break;
                case 0x4A:
                    BIT(R.d, 0x02);
                    Tick(8);
                    break;
                case 0x4B:
                    BIT(R.e, 0x02);
                    Tick(8);
                    break;
                case 0x4C:
                    BIT(R.h, 0x02);
                    Tick(8);
                    break;
                case 0x4D:
                    BIT(R.l, 0x02);
                    Tick(8);
                    break;
                case 0x4E:
                    BIT(M[R.hl], 0x02);
                    Tick(16);
                    break;
                case 0x4F:
                    BIT(R.a, 0x02);
                    Tick(8);
                    break;
                case 0x50:
                    BIT(R.b, 0x04);
                    Tick(8);
                    break;
                case 0x51:
                    BIT(R.c, 0x04);
                    Tick(8);
                    break;
                case 0x52:
                    BIT(R.d, 0x04);
                    Tick(8);
                    break;
                case 0x53:
                    BIT(R.e, 0x04);
                    Tick(8);
                    break;
                case 0x54:
                    BIT(R.h, 0x04);
                    Tick(8);
                    break;
                case 0x55:
                    BIT(R.l, 0x04);
                    Tick(8);
                    break;
                case 0x56:
                    BIT(M[R.hl], 0x04);
                    Tick(16);
                    break;
                case 0x57:
                    BIT(R.a, 0x04);
                    Tick(8);
                    break;
                case 0x58:
                    BIT(R.b, 0x08);
                    Tick(8);
                    break;
                case 0x59:
                    BIT(R.c, 0x08);
                    Tick(8);
                    break;
                case 0x5A:
                    BIT(R.d, 0x08);
                    Tick(8);
                    break;
                case 0x5B:
                    BIT(R.e, 0x08);
                    Tick(8);
                    break;
                case 0x5C:
                    BIT(R.h, 0x08);
                    Tick(8);
                    break;
                case 0x5D:
                    BIT(R.l, 0x08);
                    Tick(8);
                    break;
                case 0x5E:
                    BIT(M[R.hl], 0x08);
                    Tick(16);
                    break;
                case 0x5F:
                    BIT(R.a, 0x08);
                    Tick(8);
                    break;
                case 0x60:
                    BIT(R.b, 0x10);
                    Tick(8);
                    break;
                case 0x61:
                    BIT(R.c, 0x10);
                    Tick(8);
                    break;
                case 0x62:
                    BIT(R.d, 0x10);
                    Tick(8);
                    break;
                case 0x63:
                    BIT(R.e, 0x10);
                    Tick(8);
                    break;
                case 0x64:
                    BIT(R.h, 0x10);
                    Tick(8);
                    break;
                case 0x65:
                    BIT(R.l, 0x10);
                    Tick(8);
                    break;
                case 0x66:
                    BIT(M[R.hl], 0x10);
                    Tick(16);
                    break;
                case 0x67:
                    BIT(R.a, 0x10);
                    Tick(8);
                    break;
                case 0x68:
                    BIT(R.b, 0x20);
                    Tick(8);
                    break;
                case 0x69:
                    BIT(R.c, 0x20);
                    Tick(8);
                    break;
                case 0x6A:
                    BIT(R.d, 0x20);
                    Tick(8);
                    break;
                case 0x6B:
                    BIT(R.e, 0x20);
                    Tick(8);
                    break;
                case 0x6C:
                    BIT(R.h, 0x20);
                    Tick(8);
                    break;
                case 0x6D:
                    BIT(R.l, 0x20);
                    Tick(8);
                    break;
                case 0x6E:
                    BIT(M[R.hl], 0x20);
                    Tick(16);
                    break;
                case 0x6F:
                    BIT(R.a, 0x20);
                    Tick(8);
                    break;
                case 0x70:
                    BIT(R.b, 0x04);
                    Tick(8);
                    break;
                case 0x71:
                    BIT(R.c, 0x04);
                    Tick(8);
                    break;
                case 0x72:
                    BIT(R.d, 0x04);
                    Tick(8);
                    break;
                case 0x73:
                    BIT(R.e, 0x04);
                    Tick(8);
                    break;
                case 0x74:
                    BIT(R.h, 0x04);
                    Tick(8);
                    break;
                case 0x75:
                    BIT(R.l, 0x04);
                    Tick(8);
                    break;
                case 0x76:
                    BIT(M[R.hl], 0x04);
                    Tick(16);
                    break;
                case 0x77:
                    BIT(R.a, 0x04);
                    Tick(8);
                    break;
                case 0x78:
                    BIT(R.b, 0x80);
                    Tick(8);
                    break;
                case 0x79:
                    BIT(R.c, 0x80);
                    Tick(8);
                    break;
                case 0x7A:
                    BIT(R.d, 0x80);
                    Tick(8);
                    break;
                case 0x7B:
                    BIT(R.e, 0x80);
                    Tick(8);
                    break;
                case 0x7C:
                    BIT(R.h, 0x80);
                    Tick(8);
                    break;
                case 0x7D:
                    BIT(R.l, 0x80);
                    Tick(8);
                    break;
                case 0x7E:
                    BIT(M[R.hl], 0x80);
                    Tick(16);
                    break;
                case 0x7F:
                    BIT(R.a, 0x80);
                    Tick(8);
                    break;
                case 0x80:
                    RES(&rp->b, 0x01);
                    Tick(8);
                    break;
                case 0x81:
                    RES(&rp->c, 0x01);
                    Tick(8);
                    break;
                case 0x82:
                    RES(&rp->d, 0x01);
                    Tick(8);
                    break;
                case 0x83:
                    RES(&rp->e, 0x01);
                    Tick(8);
                    break;
                case 0x84:
                    RES(&rp->h, 0x01);
                    Tick(8);
                    break;
                case 0x85:
                    RES(&rp->l, 0x01);
                    Tick(8);
                    break;
                case 0x86:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x01);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0x87:
                    RES(&rp->a, 0x01);
                    Tick(8);
                    break;
                case 0x88:
                    RES(&rp->b, 0x02);
                    Tick(8);
                    break;
                case 0x89:
                    RES(&rp->c, 0x02);
                    Tick(8);
                    break;
                case 0x8A:
                    RES(&rp->d, 0x02);
                    Tick(8);
                    break;
                case 0x8B:
                    RES(&rp->e, 0x02);
                    Tick(8);
                    break;
                case 0x8C:
                    RES(&rp->h, 0x02);
                    Tick(8);
                    break;
                case 0x8D:
                    RES(&rp->l, 0x02);
                    Tick(8);
                    break;
                case 0x8E:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x02);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0x8F:
                    RES(&rp->a, 0x02);
                    Tick(8);
                    break;
                case 0x90:
                    RES(&rp->b, 0x04);
                    Tick(8);
                    break;
                case 0x91:
                    RES(&rp->c, 0x04);
                    Tick(8);
                    break;
                case 0x92:
                    RES(&rp->d, 0x04);
                    Tick(8);
                    break;
                case 0x93:
                    RES(&rp->e, 0x04);
                    Tick(8);
                    break;
                case 0x94:
                    RES(&rp->h, 0x04);
                    Tick(8);
                    break;
                case 0x95:
                    RES(&rp->l, 0x04);
                    Tick(8);
                    break;
                case 0x96:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x04);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0x97:
                    RES(&rp->a, 0x04);
                    Tick(8);
                    break;
                case 0x98:
                    RES(&rp->b, 0x08);
                    Tick(8);
                    break;
                case 0x99:
                    RES(&rp->c, 0x08);
                    Tick(8);
                    break;
                case 0x9A:
                    RES(&rp->d, 0x08);
                    Tick(8);
                    break;
                case 0x9B:
                    RES(&rp->e, 0x08);
                    Tick(8);
                    break;
                case 0x9C:
                    RES(&rp->h, 0x08);
                    Tick(8);
                    break;
                case 0x9D:
                    RES(&rp->l, 0x08);
                    Tick(8);
                    break;
                case 0x9E:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x08);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0x9F:
                    RES(&rp->a, 0x08);
                    Tick(8);
                    break;
                case 0xA0:
                    RES(&rp->b, 0x10);
                    Tick(8);
                    break;
                case 0xA1:
                    RES(&rp->c, 0x10);
                    Tick(8);
                    break;
                case 0xA2:
                    RES(&rp->d, 0x10);
                    Tick(8);
                    break;
                case 0xA3:
                    RES(&rp->e, 0x10);
                    Tick(8);
                    break;
                case 0xA4:
                    RES(&rp->h, 0x10);
                    Tick(8);
                    break;
                case 0xA5:
                    RES(&rp->l, 0x10);
                    Tick(8);
                    break;
                case 0xA6:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x10);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xA7:
                    RES(&rp->a, 0x10);
                    Tick(8);
                    break;
                case 0xA8:
                    RES(&rp->b, 0x20);
                    Tick(8);
                    break;
                case 0xA9:
                    RES(&rp->c, 0x20);
                    Tick(8);
                    break;
                case 0xAA:
                    RES(&rp->d, 0x20);
                    Tick(8);
                    break;
                case 0xAB:
                    RES(&rp->e, 0x20);
                    Tick(8);
                    break;
                case 0xAC:
                    RES(&rp->h, 0x20);
                    Tick(8);
                    break;
                case 0xAD:
                    RES(&rp->l, 0x20);
                    Tick(8);
                    break;
                case 0xAE:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x20);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xAF:
                    RES(&rp->a, 0x20);
                    Tick(8);
                    break;
                case 0xB0:
                    RES(&rp->b, 0x04);
                    Tick(8);
                    break;
                case 0xB1:
                    RES(&rp->c, 0x04);
                    Tick(8);
                    break;
                case 0xB2:
                    RES(&rp->d, 0x04);
                    Tick(8);
                    break;
                case 0xB3:
                    RES(&rp->e, 0x04);
                    Tick(8);
                    break;
                case 0xB4:
                    RES(&rp->h, 0x04);
                    Tick(8);
                    break;
                case 0xB5:
                    RES(&rp->l, 0x04);
                    Tick(8);
                    break;
                case 0xB6:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x04);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xB7:
                    RES(&rp->a, 0x04);
                    Tick(8);
                    break;
                case 0xB8:
                    RES(&rp->b, 0x80);
                    Tick(8);
                    break;
                case 0xB9:
                    RES(&rp->c, 0x80);
                    Tick(8);
                    break;
                case 0xBA:
                    RES(&rp->d, 0x80);
                    Tick(8);
                    break;
                case 0xBB:
                    RES(&rp->e, 0x80);
                    Tick(8);
                    break;
                case 0xBC:
                    RES(&rp->h, 0x80);
                    Tick(8);
                    break;
                case 0xBD:
                    RES(&rp->l, 0x80);
                    Tick(8);
                    break;
                case 0xBE:
                    R.imm8 = M[R.hl];
                    RES(&rp->imm8, 0x80);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xBF:
                    RES(&rp->a, 0x80);
                    Tick(8);
                    break;
                case 0xC0:
                    SET(&rp->b, 0x01);
                    Tick(8);
                    break;
                case 0xC1:
                    SET(&rp->c, 0x01);
                    Tick(8);
                    break;
                case 0xC2:
                    SET(&rp->d, 0x01);
                    Tick(8);
                    break;
                case 0xC3:
                    SET(&rp->e, 0x01);
                    Tick(8);
                    break;
                case 0xC4:
                    SET(&rp->h, 0x01);
                    Tick(8);
                    break;
                case 0xC5:
                    SET(&rp->l, 0x01);
                    Tick(8);
                    break;
                case 0xC6:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x01);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xC7:
                    SET(&rp->a, 0x01);
                    Tick(8);
                    break;
                case 0xC8:
                    SET(&rp->b, 0x02);
                    Tick(8);
                    break;
                case 0xC9:
                    SET(&rp->c, 0x02);
                    Tick(8);
                    break;
                case 0xCA:
                    SET(&rp->d, 0x02);
                    Tick(8);
                    break;
                case 0xCB:
                    SET(&rp->e, 0x02);
                    Tick(8);
                    break;
                case 0xCC:
                    SET(&rp->h, 0x02);
                    Tick(8);
                    break;
                case 0xCD:
                    SET(&rp->l, 0x02);
                    Tick(8);
                    break;
                case 0xCE:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x02);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xCF:
                    SET(&rp->a, 0x02);
                    Tick(8);
                    break;
                case 0xD0:
                    SET(&rp->b, 0x04);
                    Tick(8);
                    break;
                case 0xD1:
                    SET(&rp->c, 0x04);
                    Tick(8);
                    break;
                case 0xD2:
                    SET(&rp->d, 0x04);
                    Tick(8);
                    break;
                case 0xD3:
                    SET(&rp->e, 0x04);
                    Tick(8);
                    break;
                case 0xD4:
                    SET(&rp->h, 0x04);
                    Tick(8);
                    break;
                case 0xD5:
                    SET(&rp->l, 0x04);
                    Tick(8);
                    break;
                case 0xD6:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x04);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xD7:
                    SET(&rp->a, 0x04);
                    Tick(8);
                    break;
                case 0xD8:
                    SET(&rp->b, 0x80);
                    Tick(8);
                    break;
                case 0xD9:
                    SET(&rp->c, 0x80);
                    Tick(8);
                    break;
                case 0xDA:
                    SET(&rp->d, 0x80);
                    Tick(8);
                    break;
                case 0xDB:
                    SET(&rp->e, 0x80);
                    Tick(8);
                    break;
                case 0xDC:
                    SET(&rp->h, 0x80);
                    Tick(8);
                    break;
                case 0xDD:
                    SET(&rp->l, 0x80);
                    Tick(8);
                    break;
                case 0xDE:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x80);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xDF:
                    SET(&rp->a, 0x80);
                    Tick(8);
                    break;
                case 0xE0:
                    SET(&rp->b, 0x10);
                    Tick(8);
                    break;
                case 0xE1:
                    SET(&rp->c, 0x10);
                    Tick(8);
                    break;
                case 0xE2:
                    SET(&rp->d, 0x10);
                    Tick(8);
                    break;
                case 0xE3:
                    SET(&rp->e, 0x10);
                    Tick(8);
                    break;
                case 0xE4:
                    SET(&rp->h, 0x10);
                    Tick(8);
                    break;
                case 0xE5:
                    SET(&rp->l, 0x10);
                    Tick(8);
                    break;
                case 0xE6:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x10);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xE7:
                    SET(&rp->a, 0x10);
                    Tick(8);
                    break;
                case 0xE8:
                    SET(&rp->b, 0x20);
                    Tick(8);
                    break;
                case 0xE9:
                    SET(&rp->c, 0x20);
                    Tick(8);
                    break;
                case 0xEA:
                    SET(&rp->d, 0x20);
                    Tick(8);
                    break;
                case 0xEB:
                    SET(&rp->e, 0x20);
                    Tick(8);
                    break;
                case 0xEC:
                    SET(&rp->h, 0x20);
                    Tick(8);
                    break;
                case 0xED:
                    SET(&rp->l, 0x20);
                    Tick(8);
                    break;
                case 0xEE:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x20);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xEF:
                    SET(&rp->a, 0x20);
                    Tick(8);
                    break;
                case 0xF0:
                    SET(&rp->b, 0x40);
                    Tick(8);
                    break;
                case 0xF1:
                    SET(&rp->c, 0x40);
                    Tick(8);
                    break;
                case 0xF2:
                    SET(&rp->d, 0x40);
                    Tick(8);
                    break;
                case 0xF3:
                    SET(&rp->e, 0x40);
                    Tick(8);
                    break;
                case 0xF4:
                    SET(&rp->h, 0x40);
                    Tick(8);
                    break;
                case 0xF5:
                    SET(&rp->l, 0x40);
                    Tick(8);
                    break;
                case 0xF6:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x40);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xF7:
                    SET(&rp->a, 0x40);
                    Tick(8);
                    break;
                case 0xF8:
                    SET(&rp->b, 0x80);
                    Tick(8);
                    break;
                case 0xF9:
                    SET(&rp->c, 0x80);
                    Tick(8);
                    break;
                case 0xFA:
                    SET(&rp->d, 0x80);
                    Tick(8);
                    break;
                case 0xFB:
                    SET(&rp->e, 0x80);
                    Tick(8);
                    break;
                case 0xFC:
                    SET(&rp->h, 0x80);
                    Tick(8);
                    break;
                case 0xFD:
                    SET(&rp->l, 0x80);
                    Tick(8);
                    break;
                case 0xFE:
                    R.imm8 = M[R.hl];
                    SET(&rp->imm8, 0x80);
                    M[R.hl] = R.imm8;
                    Tick(16);
                    break;
                case 0xFF:
                    SET(&rp->a, 0x80);
                    Tick(8);
                    break;
                }
            }
        }
    }
}

