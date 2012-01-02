using Cs360Emu.Core.Cpu.Dynarec;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cs360Emu.Core.Cpu;
using System.Reflection.Emit;

namespace Cs360Emu.Tests
{
	[TestClass]
	public class CompilerStateTest
	{
		[TestMethod]
		public void CreateDelegateTest()
		{
			var CpuThreadState = new CpuThreadState();
			var CompilerState = new CompilerState();
			CpuThreadState.r1 = 1000000000000000000;
			CompilerState.SaveGpr(0, () =>
			{
				CompilerState.LoadGpr(1);
				CompilerState.LoadImm(999);
				CompilerState.Emit(OpCodes.Add);
			});
			CompilerState.Ret();
			CompilerState.CreateDelegate()(CpuThreadState);
			Assert.AreEqual(1000000000000000999, CpuThreadState.r0);
		}
	}
}
