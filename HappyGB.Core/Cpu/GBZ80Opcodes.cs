using System;
using System.Runtime.CompilerServices;

using HappyGB.Core.Memory;

namespace HappyGB.Core.Cpu
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

        /// <summary>
        /// Load the 16 bits under the PC into the given register.
        /// </summary>
        /// <param name="reg">16bit register to load to</param>
        public void LD_rr_imm16(ushort* reg)
        {
            *reg = Fetch16();
        }

        /// <summary>
        /// Loads the value in the given register to the memory location under the PC 
        /// </summary>
        /// <param name="reg"></param>
        public void LD_imm16_rr(ushort reg)
        {
            M.Write16(Fetch16(), reg);
        }

        /// <summary>
        /// Sets the stack pointer to be equal to the HL register.
        /// </summary>
        public void LD_sp_hl()
        {
            R.sp = R.hl;
        }

        /// <summary>
        /// Sets the HL register to be equal to the value under the stack pointer.
        /// Sets half carry and carry flags accordingly.
        /// </summary>
        public void LD_hl_sp_imm()
        {
            byte immediateValue = Fetch8();

            if ((((immediateValue & 0xF)+ (R.sp & 0xF)) & 0x10) == 0x10) //set half carry if a half carry happened
                R.HCF = true;
            else R.HCF = false;

            if (((immediateValue + (R.sp & 0x00FF)) & 0x100) == 0x100) //set carry if a carry happened
                R.CF = true;
            else R.CF = false;

            //reset z and n -- gbcpuman p77
            R.ZF = false;
            R.NF = false;

            sbyte ivAsSignedByte = unchecked((sbyte)immediateValue); 

            int offsetStackPointerValue = R.sp;
            offsetStackPointerValue += ivAsSignedByte;

            R.hl = (ushort)offsetStackPointerValue; //Should be HL not SP, since this is a 'ld hl'. Doh!
        }

        /// <summary>
        /// Decrements the stack pointer and writes a value to the stack.
        /// </summary>
        /// <param name="reg">The register to push onto the stack.</param>
        public void PUSH(ushort reg)
        {
            R.sp -= 2;
            M.Write16(R.sp, reg);
        }

        /// <summary>
        /// Copies the value at the stack pointer to the given regiser, then increments the stack pointer.
        /// </summary>
        /// <param name="reg"></param>
        public void POP(ushort* reg)
        {
            *reg = M.Read16(R.sp);
            R.sp += 2;
        }

        #endregion

        #region ALU 8bit
        public void ADD_A_n(byte nv)
        {
            R.HCF = (((R.a & 0x0F) + (nv & 0x0F)) & 0x10) == 0x10;

            R.NF = false;

            int res = R.a + nv;
            if((res & 0x100) == 0x100)
                R.CF = true;

            if(res == 0)
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

        public void SUB_A_n(byte subtractValue, bool withCarryFlag = false)
        {
            ///FIXME: Is this correct too? if a > b we dont need to borrow in a-b.

            //Adjust the subtract value based on the carry flag.
            ushort cfAdjustedValue = subtractValue;
            if (withCarryFlag && R.CF)
                cfAdjustedValue++;

            ushort resultShort = (ushort)(((ushort)R.a) - cfAdjustedValue);

            //Set the carry ('set if no borrow')
            R.CF = (resultShort & 0x100) == 0x100;

            //Set the half carry ('set if no borrow from bit 4')
            int hcOperand = (cfAdjustedValue & 0x0F); 
            int hcA = (R.a & 0x0F);
            R.HCF = ((hcOperand - hcA) & 0x10) == 0x10;

            //NF is always set.
            R.NF = true;

            //subtract the new value from A
            R.a = (byte)resultShort;

            //ZF if flag is zero.
            R.ZF = (R.a == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SBC_A_n(byte nc)
        {
            SUB_A_n(nc, true);
        }

        public void AND_A_n(byte n)
        {
            R.NF = R.CF = false;
            R.HCF = true;

            R.a = (byte)(R.a & n);

            R.ZF = R.a == 0;
        }

        public void OR_A_n(byte n)
        {
            R.NF = R.CF = R.HCF = false;
            R.a = (byte)(R.a | n);

            R.ZF = R.a == 0;
        }

        public void XOR_A_n(byte n)
        {
            R.NF = R.CF = R.HCF = false;
            R.a = (byte)(R.a ^ n);

            R.ZF = R.a == 0;
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
            R.HCF = ((*n & 0x0F) == 0x0F); //Only way hc gets set.
            R.NF = false;
            (*n)++;

            R.ZF = *n == 0;

            //C: Not affected.
        }

        public void DEC_n(byte* n)
        {
            R.HCF = ((*n & 0x0F) == 0x00); //FIXME: This seems wrong.
            R.NF = true;
            (*n)--;

            R.ZF = *n == 0;

            //C: Not affected.
        }

        public void DAA()
        {
            if (R.HCF || (R.a & 0x0F) > 0x09)
            {
                //R.HCF = true; //?
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
            R.a ^= 0xFF; //From Z80Heaven: Same as XORing A with $FF or subtracting
                         //A from $FF.
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
            R.ZF = (test & mask) != mask; // Set if bit b of register r is 0.
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
            // FIXME: This isn't supported yet, but it shouldn't be hit in normal execution.
            // STOP suspends the CPU & turns off the screen until a button is pressed.
            // IIRC there's a bug / quirk with the CPU where STOP instructions consume
            // the next instruction as well.
            // (e.g. it's in GBCPUMAN as opcode '10 00'.)
        }

        #endregion

        #region Jump
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int JP_relative(bool cond)
        {
            byte relAddrUnsigned = Fetch8();
            sbyte relAddrSigned = unchecked((sbyte)relAddrUnsigned);
            ushort addr = (ushort)(R.pc + relAddrSigned); 
            int res = JP(addr, cond);
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

                this.stackTrace.Push(newPc);

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

            if (this.stackTrace.Count != 0)
            {
                this.stackTrace.Pop();
            }
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
            PUSH(R.pc);
            R.pc = rstVector;
        }

        #endregion
    }
}

