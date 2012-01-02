using Cs360Emu.Core.Cpu.Disassembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Cs360Emu.Tests
{
	[TestClass]
	public class DisassemblerTest
	{
		[TestMethod()]
		public void DissasembleTest()
		{
			var Disassembler = new Disassembler();
			Assert.AreEqual("addis r11, r0, 0x8202", Disassembler.DissasembleSimple(0x3D608202));
			Assert.AreEqual("add r29, r5, r6", Disassembler.DissasembleSimple(0x7FA53214));
			Assert.AreEqual("add. r11, r11, r10", Disassembler.DissasembleSimple(0x7D6B5215));
			Assert.AreEqual("mflr r12", Disassembler.DissasembleSimple(0x7D8802A6));
			Assert.AreEqual("mtlr r12", Disassembler.DissasembleSimple(0x7D8803A6));
			Assert.AreEqual("blr", Disassembler.DissasembleSimple(0x4E800020));
			
		}
	}
}
