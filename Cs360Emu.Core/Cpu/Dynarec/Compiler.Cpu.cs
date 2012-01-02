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
		public void add()
		{
			CompilerState.SaveGpr(Instruction.D, () =>
			{
				CompilerState.LoadGpr(Instruction.A);
				CompilerState.LoadGpr(Instruction.B);
				CompilerState.Emit(OpCodes.Add);
			});
		}

		public void addi()
		{
			CompilerState.SaveGpr(Instruction.D, () =>
			{
				CompilerState.LoadGpr(Instruction.A);
				CompilerState.LoadImm(Instruction.Imm16Signed);
				CompilerState.Emit(OpCodes.Add);
			});
		}

		public void mflr()
		{
			CompilerState.SaveGpr(Instruction.S, () =>
			{
				CompilerState.Load8(CompilerState.LinkRegisterFieldInfo);
			});
		}

		public void mtlr()
		{
			CompilerState.Save8(CompilerState.LinkRegisterFieldInfo, () =>
			{
				CompilerState.LoadGpr(Instruction.S);
			});
		}
	}
}
