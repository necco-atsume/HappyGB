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
            cpu = new GBZ80(memory);
            cpu.Reset();
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
        public void Opcode_SRL_n_ShouldPut0InBit7_AndShiftBitsRight_AndPutBitZeroInCarryFlag()
        {
            //Test cases: 11111111 (0xFF) -> 01111111 (0x7F), C=1
            //            00000000 (0x00) -> 00000000 (0x00), C=0
            //            10101010 (0xAA) -> 01010101 (0x55), C=0
            //            01010101 (0x55) -> 00101010 (0x2A), C=1

            byte[] testCases = { 0xFF, 0x00, 0xAA, 0x55 };
            byte[] expected = { 0x7F, 0x00, 0x55, 0x2A };
            bool[] expectedCarryFlags = { true, false, false, true };

            for (int i = 0; i < testCases.Length; i++)
            {
                byte testCase = testCases[i];

                cpu.SRL(&testCase); //Since it does the shift in place...

                Assert.AreEqual(expectedCarryFlags[i], cpu.Registers.CF);
                Assert.AreEqual(expected[i], testCase);
            }
        }
    }
}
