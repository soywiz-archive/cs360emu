using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu.Dynarec
{
	unsafe public partial class Compiler
	{
		public Compiler(CompilerState CompilerState)
		{
			this.CompilerState = CompilerState;
		}

		public CompilerState CompilerState;

		public Instruction Instruction
		{
			get
			{
				return CompilerState.CurrentInstruction;
			}
		}
	}
}
