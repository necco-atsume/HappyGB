using System;
using System.Runtime.CompilerServices;

namespace HappyGB.Core
{
    public unsafe partial class GBZ80
    {
        #region LD 8bit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LD(byte* n, byte v)
        {
            *n = v;
        }
        #endregion

        #region LD 16bit
        public void LD_rr_imm16(ushort* reg)
        {
            *reg = Fetch16();
        }

        public void LD_imm16_rr(ushort reg)
        {
            M.Write16(Fetch16(), reg);
        }

        public void LD_sp_hl()
        {
            R.sp = R.hl;
        }

        public void LD_hl_sp_imm()
        {
            byte r = Fetch8();
            if (((r + (R.sp & 0x000F)) & 0x10) == 0x10)
                R.HCF = true;
            else R.HCF = false;
            if (((r + (R.sp & 0x00FF)) & 0x100) == 0x100)
                R.CF = true;
            else R.CF = false;
            sbyte sr = unchecked((sbyte)r);
            int sp = R.sp;
            sp += sr;
            R.sp = (ushort)sp;
        }

        public void PUSH(ushort reg)
        {
            R.sp -= 2;
            M.Write16(R.sp, reg);
        }

        public void POP(ushort* reg)
        {
            *reg = M.Read16(R.sp);
            R.sp += 2;
        }

        #endregion

        #region ALU 8bit
        public void ADD_A_n(byte nv)
        {
            R.HCF = (((R.a & 0x0F) + (nv & 0x0F)) & 0x100) == 0x100;

            R.NF = false;

            int res = R.a + nv;
            if((res & 0x100) == 0x100)
                R.CF = true;

            if(nv == 0)
                R.ZF = true;

            R.a = (byte)(0xFF & res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ADC_A_n(byte nc)
        {
            //FIXME: Accurate???
            if(R.CF)
                nc++;
            ADD_A_n(nc);
        }

        public void SUB_A_n(byte nv)
        {

            ///FIXME: Prolly slow. find some crazy bitwise stupid shit to doi inst.
            ///FIXME: Is this correct too? if a > b we dont need to borrow in a-b.
            int hcOperand = (nv & 0x0F);
            int hcA = (R.a & 0x0F);
            R.HCF = (hcOperand < hcA);
            R.CF = nv < R.a;
            R.NF = true;

            R.a -= nv;

            R.ZF = (R.a == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SBC_A_n(byte nc)
        {
            if(R.CF)
                nc--;
            SUB_A_n(nc);
        }

        public void AND_A_n(byte n)
        {
            R.NF = R.CF = false;
            R.HCF = true;

            R.a = (byte)(R.a & n);

            if(R.a == 0)
                R.ZF = true;
            else
                R.ZF = false;
        }

        public void OR_A_n(byte n)
        {
            R.NF = R.CF = R.HCF = false;
            R.a = (byte)(R.a | n);

            if(R.a == 0)
                R.ZF = true;
            else
                R.ZF = false;
        }

        public void XOR_A_n(byte n)
        {
            R.NF = R.CF = R.HCF = false;
            R.a = (byte)(R.a ^ n);

            if(R.a == 0)
                R.ZF = true;
            else
                R.ZF = false;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CP_A_n(byte n)
        {
            //TODO: If we need to we can optimize this.
            byte a = R.a;
            SUB_A_n(n);
            R.a = a;
        }

        public void INC_n(byte* n)
        {
            //FIXME: Dereference pointer three times. Is this no good?

            R.HCF = ((*n & 0x0F) == 0x0F); //Only way hc gets set.
            R.NF = false;
            (*n)++;

            if(*n == 0)
                R.ZF = true;
            else
                R.ZF = false;
        }

        public void DEC_n(byte* n)
        {
            R.HCF = ((*n & 0x0F) == 0x00);
            R.NF = true;
            (*n)--;

            if(*n == 0)
                R.ZF = true;
            else
                R.ZF = false;
        }

        public void DAA()
        {
            if ((R.a & 0x0F) > 0x09)
            {
                R.HCF = true;
                R.a += 0x06;
            }
            if (R.CF || ((R.a & 0xF0) > 0x90))
            {
                R.CF = true;
                R.a += 0x60;
            }
        }

        public void CPL()
        {
            R.NF = R.HCF = true;
            R.a ^= 0xFF;
        }
        #endregion ALU 8bit

        #region ALU 16bit

        public void ADD_HL_rr(ushort n)
        {
            ushort hcResult = (ushort)((0x0FFF & n) + (0x0FFF & R.hl));
            R.HCF = (hcResult & 0x1000) == 0x1000;
            int cResult = n + R.hl;
            R.CF = (cResult & 0x10000) == 0x10000;
            R.hl = (ushort)(cResult & 0xFFFF);
            R.NF = false;
        }

        public void ADD_SP_n(byte n)
        {
            sbyte v = unchecked((sbyte)n);
            R.ZF = R.NF = false;
            //FIXME: Not specific on how C/HC flags get set.
            R.sp = (ushort)unchecked(R.sp + v);
        }

        public void INC_nn(ushort* nn)
        {
            ushort nv = *nn;
            nv++;
            *nn = nv;
        }

        public void DEC_nn(ushort* nn)
        {
            ushort nv = *nn;
            nv--;
            *nn = nv;
        }

        #endregion ALU 16bit

        #region ROT / Shift

        public void RLC(byte* n)
        {
            bool msb = (*n & 0x80) == 0x80;
            byte nv = (byte)(*n << 1);
            if (msb)
                nv++;
            *n = nv;
            R.CF = msb;
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
        }

        public void RL(byte* n)
        {
            bool oldCarry = R.CF;
            R.NF = R.HCF = false;
            R.CF = (*n & 0x80) == 0x80;
            byte nv = (byte)(*n << 1);
            if (oldCarry)
                nv = (byte)(nv | 0x01);
            if (nv == 0)
                R.ZF = true;
            *n = nv;
        }

        public void RRC(byte* n)
        {
            bool lsb = (*n & 0x01) == 0x01;
            byte nv = (byte)(*n >> 1);
            if (lsb)
                nv |= 0x80;
            *n = nv;
            R.CF = lsb;
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
        }

        public void RR(byte* n)
        {
            bool oldCarry = R.CF;
            bool lsb = (*n & 0x01) == 0x01;
            byte nv = (byte)(*n >> 1);
            R.CF = lsb;
            if (oldCarry)
                nv |= 0x80;
            *n = nv;
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
        }

        public void SL(byte* n)
        {
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
            R.CF = (0x80 & *n) == 0x80;
            *n = (byte)(*n << 1);
        }

        public void SR(byte* n)
        {
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
            R.CF = (0x10 & *n) == 0x10;
            *n = (byte)(*n >> 1);
        }

        public void SLA(byte* n)
        {
            bool lsb = (0x01 & *n) == 0x01;
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
            R.CF = (0x80 & *n) == 0x80;

            *n = (byte)(*n << 1);
            *n = (byte) (*n | (lsb? 0x01 : 0x00));
        }

        public void SRA(byte* n)
        {
            bool msb = (0x80 & *n) == 0x80;
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
            R.CF = (0x10 & *n) == 0x10;
            *n = (byte)(*n >> 1);
            if (msb)
                *n |= 0x80;
        }


        public void SRL(byte* n)
        {
            R.NF = R.HCF = false;
            R.ZF = *n == 0;
            R.CF = (0x10 & *n) == 0x10;
            *n = (byte)(*n >> 1);
        }

        public void SWAP(byte* n)
        {
            R.CF = R.HCF = R.NF = false;
            R.ZF = *n == 0;

            byte sl = (byte)((*n >> 4) & 0x0F);
            byte sr = (byte)((*n << 4) & 0xF0);
            *n = (byte)(sl | sr);
        }

        #endregion

        #region Singlebit Ops

        public void BIT(byte test, byte mask)
        {
            R.NF = false;
            R.HCF = true;
            R.ZF = (test & mask) == mask;
        }

        public void SET(byte* n, byte mask)
        {
            *n |= mask;
        }

        public void RES(byte* n, byte mask)
        {
            *n = (byte)unchecked(*n & ~mask);
        }

        #endregion

        #region CPU control

        public void CCF()
        {
            R.CF = !R.CF;
            R.NF = R.HCF = false;
            R.UpdateAFFromFlags();
        }

        public void SCF()
        { 
            R.CF = true;
            R.NF = R.HCF = false;
            R.UpdateAFFromFlags();
        }

        public void HLT()
        {
            halted = true;
        }

        public void STOP()
        {
            Fetch8();
            throw new NotImplementedException("Stop can't be supported til we get gamepads.");
        }

        #endregion

        #region Jump
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int JP_relative(bool cond)
        {
            //System.Diagnostics.Debug.WriteLine("-- jr pc:" + R.pc.ToString("X"));
            byte relAddrUnsigned = Fetch8();
            sbyte relAddrSigned = unchecked((sbyte)relAddrUnsigned);
            ushort addr = (ushort)(R.pc + relAddrSigned); 
            int res = JP(addr, cond);
            //System.Diagnostics.Debug.WriteLine("new pc:" + R.pc.ToString("X")+" --");
            return res;
        }

        public int JP(ushort addr, bool cond)
        {
            if (cond)
            {
                R.pc = addr;
                return 4;
            }
            else return 0;
        }

        public int CALL(bool cond)
        {
            if (cond)
            {
                R.sp -= 2;
                ushort newPc = Fetch16();
                M.Write16(R.sp, R.pc);
                R.pc = newPc;
                return 12;
            }
            else
            {
                Fetch16(); //Scoot forward 2 boops.
                return 0;
            }
        }

        public void RET()
        {
            R.pc = M.Read16(R.sp); 
            R.sp += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int RET_cond(bool cond)
        {
            if (cond)
            {
                RET();
                return 12;
            }
            else
            {
                return 0;
            }
        }

        public void RST(ushort rstVector)
        {
            R.sp -= 2;
            M.Write16(R.sp, R.pc);
            R.pc = rstVector;
        }

        #endregion
    }
}

