using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Cs360Emu.Core.Cpu;
using Cs360Emu.Core.Cpu.Disassembler;
using Cs360Emu.Core.Cpu.Dynarec;
using Cs360Emu.Core.Memory;
using Cs360Emu.Hle.Formats;
using CSharpUtils.Extensions;

namespace Cs360Emu
{
	class Program
	{
		static void Main(string[] args)
		{
			Test1();
			Console.ReadKey();
		}

		static void Test1()
		{
			var Memory = new Xbox360MemoryNormal();
			var Xex = new Xex();
			var CpuThreadState = new CpuThreadState();
			Xex.LoadHeader(File.OpenRead("../../../TestInput/test.xex"));
			Xex.Pe.LoadSectionsTo(new Xbox360MemoryStream(Memory));
			CpuThreadState.pc = Xex.Pe.PeOptionalHeader.AddressOfEntryPoint + Xex.Pe.PeOptionalHeader.ImageBase;
			Console.WriteLine("{0:X}: {1:X}", CpuThreadState.pc, Memory.Read4(CpuThreadState.pc));
		}

		static void Test2()
		{
			//new DisassemblerTest().DissasembleTest();
			/*
			Console.WriteLine(Disassembler.DissasembleSimple(0x3D608202));
			Console.WriteLine(Disassembler.DissasembleSimple(0x7C7E1B78));
			Console.WriteLine(Disassembler.DissasembleSimple(0x7FA53214));
			Console.WriteLine(Disassembler.DissasembleSimple(0x7D8802A6));
			*/

			Console.WriteLine(Disassembler.DissasembleSimple(0x4E800020));

			var CpuThreadState = new CpuThreadState();
			var CompilerState = new CompilerState();
			var Compiler = new Compiler(CompilerState);
			CompilerState.CurrentInstruction = 0x7D8802A6;
			Compiler.mflr();
			var Delegate = CompilerState.CreateDelegate();

			CpuThreadState.lr = 999;
			Delegate(CpuThreadState);

			Console.WriteLine("{0}, {1}", CpuThreadState.lr, CpuThreadState.r12);
		}
	}
}
