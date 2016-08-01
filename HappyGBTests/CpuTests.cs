using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core.Cpu;
using HappyGB.Core.Memory;
using NUnit.Framework;

namespace HappyGBTests
{
    [TestFixture]
    public unsafe class CpuOpcodeTests
    {
        private IMemoryMap memory;
        private GBZ80 cpu;

        [SetUp]
        public void CreateCpu()
        {
            memory = new StubMemoryMap();
            memory[0] = 0xFF;
            memory[1] = 0xFF;
            memory[2] = 0x11;
            memory[3] = 0x11;

            cpu = new GBZ80(memory);
            cpu.Reset();
        }

        private RegisterGroup Registers(byte a = 0, byte b = 0, byte c = 0, byte d = 0, byte e = 0, byte h = 0, byte l = 0, bool carry = false,
            bool halfCarry = false, bool zero = false, bool negative = false, ushort pc = 0, ushort sp = 0)
        {
            RegisterGroup rg = new RegisterGroup();
            rg.a = a;
            rg.b = b;
            rg.c = c;
            rg.d = d;
            rg.e = e;
            rg.h = h;
            rg.l = l;
            rg.CF = carry;
            rg.HCF = halfCarry;
            rg.ZF = zero;
            rg.NF = negative;
            rg.pc = pc;
            rg.sp = sp;
            return rg;
        }

        [TestCase]
        public void Opcode_ADD_A_n_ShouldPopulateAWithAddedValue() 
        {
            var reg = cpu.Registers;
            reg.a = 3;
            cpu.Registers = reg;

            cpu.ADD_A_n(2);

            Assert.AreEqual(cpu.Registers.a, 5);
        }

        [TestCase]
        public void Opcode_ADD_A_n_ShouldSetZeroFlagWhenResultIsZero_AndShouldSetCarryFlagWhenCarry()
        {
            var reg = cpu.Registers;
            reg.a = 0xFF;
            cpu.Registers = reg;

            cpu.ADD_A_n(1);

            Assert.AreEqual(cpu.Registers.a, 0);
            Assert.AreEqual(cpu.Registers.ZF, true);
            Assert.AreEqual(cpu.Registers.CF, true);
        }

        [TestCase]
        public void Opcode_ADD_A_n_ShouldSetHalfCarryFlagWhenHalfCarryOccurs()
        {
            cpu.Registers = Registers(a: 0x0F);

            cpu.ADD_A_n(1);

            Assert.AreEqual(0x10, cpu.Registers.a);
            Assert.IsTrue(cpu.Registers.HCF);
        }

        [TestCase]
        public void Opcode_INC_n_ShouldIncrementValue()
        {
            byte val = 0;

            cpu.INC_n(&val);

            Assert.AreEqual(1, val);
        }

        [TestCase]
        public void Opcode_INC_n_ShouldSetHalfCarryFlagWhenHalfCarry()
        {
            cpu.Registers = Registers(halfCarry: false);
            byte val = 0x0F;

            cpu.INC_n(&val);

            Assert.AreEqual(0x10, val);
            Assert.IsTrue(cpu.Registers.HCF);
        }

        [TestCase]
        public void Opcode_INC_n_ShouldUnsetSubtractFlag()
        {
            cpu.Registers = Registers(carry: true);
            byte val = 0;

            cpu.INC_n(&val);

            Assert.IsFalse(cpu.Registers.NF);
        }

        [TestCase]
        public void Opcode_INC_n_GivenOverflow_ShouldNotSetCarryFlag_AndShouldSetZeroFlag()
        {
            cpu.Registers = Registers(carry: false, zero: false);
            byte val = 0xFF;

            cpu.INC_n(&val);

            Assert.AreEqual(0, val);
            Assert.IsFalse(cpu.Registers.CF);
            Assert.IsTrue(cpu.Registers.ZF);
        }

        [TestCase]
        public void Opcode_SRL_n_ShouldPut0InBit7_AndShiftBitsRight_AndPutBitZeroInCarryFlag_AndSetZeroFlagByDefinition_AndUnsetOtherFlags()
        {
            //Test cases: 11111111 (0xFF) -> 01111111 (0x7F), C=1
            //            00000000 (0x00) -> 00000000 (0x00), C=0
            //            10101010 (0xAA) -> 01010101 (0x55), C=0
            //            01010101 (0x55) -> 00101010 (0x2A), C=1

            byte[] testCases = { 0xFF, 0x00, 0xAA, 0x55 };
            byte[] expected = { 0x7F, 0x00, 0x55, 0x2A };
            bool[] expectedCarryFlags = { true, false, false, true };
            bool[] expectedZeroFlags = { false, true, false, false }; 

            for (int i = 0; i < testCases.Length; i++)
            {
                byte testCase = testCases[i];

                cpu.SRL(&testCase); //Since it does the shift in place...

                Assert.AreEqual(expectedCarryFlags[i], cpu.Registers.CF);
                Assert.AreEqual(expected[i], testCase);
                Assert.AreEqual(expectedZeroFlags[i], cpu.Registers.ZF);
                Assert.IsFalse(cpu.Registers.HCF);
                Assert.IsFalse(cpu.Registers.NF);
            }
        }

        [TestCase]
        public void Opcode_SWAP_n_GivenNonzeroValue_ShouldSwapNibbles_AndResetFlags()
        {
            byte reg = 0xA5;
            cpu.Registers = Registers(zero: true, carry: true, halfCarry: true, negative: true);

            cpu.SWAP(&reg);

            Assert.AreEqual(reg, 0x5A);
            Assert.IsFalse(cpu.Registers.CF);
            Assert.IsFalse(cpu.Registers.HCF);
            Assert.IsFalse(cpu.Registers.NF);
            Assert.IsFalse(cpu.Registers.ZF);
        }

        [TestCase]
        public void Opcode_SWAP_n_GivenZeroValue_ShouldSetZeroFlag()
        {
            byte reg = 0x00;
            cpu.Registers = Registers(zero: true, carry: true, halfCarry: true, negative: true);

            cpu.SWAP(&reg);

            Assert.IsTrue(cpu.Registers.ZF);
        }

        [TestCase]
        public void Opcode_JP_relative_GivenFalseCondition_DoesNotJump_AndDoesNotAddCycles()
        {
            cpu.Registers = Registers(pc: 0x1000);

            var result = cpu.JP_relative(false);

            Assert.AreEqual(0x1000 + 1, cpu.Registers.pc); //Plus one for the 8-bit fetch it has to do for the relative address!
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Opcode_BIT_n_GivenBitMask_SetsBit_AndSetsZeroFlagIfNotSet_AndUnsetsSubtractFlag_AndSetsHalfCarryFlag(
            [Values(0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80)] byte bitMask,
            [Values(0x00, 0xFF)] byte testCase
        )
        {
            bool expectedZeroFlag = (testCase == 0x00);

            cpu.BIT(testCase, bitMask);

            Assert.AreEqual(expectedZeroFlag, cpu.Registers.ZF);
            Assert.IsTrue(cpu.Registers.HCF);
            Assert.IsFalse(cpu.Registers.NF);
        }

        [TestCase]
        public void Opcode_LD_rr_imm16_Loads16BitImmediateValueInto16BitLocation()
        {
            cpu.Registers = Registers(pc: 0x0000);
            ushort result = 0;

            cpu.LD_rr_imm16(&result);

            Assert.AreEqual(0xFFFF, result);
        }

        //TODO: Pull the OpcodeTable ones into their own class?

        [TestCase]
        public void OpcodeTable_JP_hl_JumpsToHLValue_AndAddsFourCycles()
        {
            cpu.Registers = Registers(h: 0x12, l: 0x34);
            var initialCycles = cpu.LocalTickCount;

            cpu.ExecuteOpcode(0xE9);

            Assert.AreEqual(4, cpu.LocalTickCount - initialCycles);
            Assert.AreEqual(0x1234, cpu.Registers.pc);
        }

        [TestCase]
        public void OpcodeTable_LD_SP_imm16_LoadsImmediateValueInto16BitRegister()
        {
            //Immediate 16 will be 0xFFFF.
            cpu.Registers = Registers(pc: 0);

            cpu.ExecuteOpcode(0x31);

            Assert.AreEqual(cpu.Registers.pc, 2);
            Assert.AreEqual(cpu.Registers.sp, 0xFFFF);
        }
    }
}
