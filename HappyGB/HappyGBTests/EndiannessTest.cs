using NUnit.Framework;
using System;

using HappyGB.Core;

namespace HappyGBTests
{
	[TestFixture]
	public class EndiannessTest
	{
		private MemoryMap mem;

		[TestFixtureSetUp]
		public void CreateMocks()
		{
			mem = new MemoryMap(null, null, null);
			mem[0xE000] = 0x0F;
			mem[0xE001] = 0xF0;
			mem[0xE002] = 0x0F;
			mem[0xE003] = 0xF0;
			mem[0xE004] = 0x0F;
			mem[0xE005] = 0xF0;
		}

		[Test]
		public void TestReads16BitValueCorrect()
		{
			mem[0xE000] = 0x0F;
			mem[0xE001] = 0xF0;
			RegisterGroup r = new RegisterGroup();
			r.hl = mem.Read16(0xE000);
			Assert.AreEqual(r.h, 0xF0);
			Assert.AreEqual(r.l, 0x0F);
		}

		[Test]
		public void TestWrites16BitValueCorrect()
		{
			RegisterGroup r = new RegisterGroup();
			r.bc = 0xA00A;
			Assert.AreEqual(r.b, 0xA0);
			mem.Write16(0xE000, r.bc);
			Assert.AreEqual(mem[0xE000], 0x0A);
			Assert.AreEqual(mem[0xE001], 0xA0);
		}

		[Test]
		public void TestReadWriteComposes()
		{
			mem[0xE000] = 0x0F;
			mem[0xE001] = 0xF0;
			RegisterGroup r = new RegisterGroup();
			r.bc = mem.Read16(0xE000);
			mem.Write16(0xE000, r.bc);
			Assert.AreEqual(mem[0xE000], 0x0F);
			Assert.AreEqual(mem[0xE001], 0xF0);
		}
	}
}

