using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cs360Emu.Core.Cpu.Disassembler;

namespace Cs360Emu
{
	class Program
	{
		static void Main(string[] args)
		{
			//new DisassemblerTest().DissasembleTest();
			//Console.WriteLine(Disassembler.Dissasemble(0x3D608202));
			//Console.WriteLine(Disassembler.Dissasemble(0x7C7E1B78));
			//Console.WriteLine(Disassembler.DissasembleSimple(0x7FA53214));
			Console.WriteLine(Disassembler.DissasembleSimple(0x7D6B5215));

			Console.ReadKey();
		}
	}
}
