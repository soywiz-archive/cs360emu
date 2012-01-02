using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu.Dynarec
{
	unsafe public sealed partial class CompilerState
	{
		public Instruction CurrentInstruction;

		private DynamicMethod DynamicMethod;
		private ILGenerator ILGenerator;

		public FieldInfo[] GprFieldInfo;
		public FieldInfo LinkRegisterFieldInfo;

		public CompilerState()
		{
			this.GprFieldInfo = new FieldInfo[32];
			for (int n = 0; n < 32; n++) this.GprFieldInfo[n] = typeof(CpuThreadState).GetField("r" + n);

			this.LinkRegisterFieldInfo = typeof(CpuThreadState).GetField("lr");

			this.DynamicMethod = new DynamicMethod("", typeof(void), new Type[] { typeof(CpuThreadState) }, Assembly.GetExecutingAssembly().ManifestModule);
			this.ILGenerator = this.DynamicMethod.GetILGenerator();
		}

		public void Save8(FieldInfo FieldInfo, Action Callback)
		{
			ILGenerator.Emit(OpCodes.Ldarg_0);
			ILGenerator.Emit(OpCodes.Ldflda, FieldInfo);
			Callback();
			ILGenerator.Emit(OpCodes.Stind_I8);
		}

		public void Load8(FieldInfo FieldInfo)
		{
			ILGenerator.Emit(OpCodes.Ldarg_0);
			ILGenerator.Emit(OpCodes.Ldflda, FieldInfo);
			ILGenerator.Emit(OpCodes.Ldind_I8);
		}

		public void SaveGpr(uint Index, Action Callback)
		{
			Save8(GprFieldInfo[Index], Callback);
		}

		public void LoadGpr(uint Index)
		{
			Load8(GprFieldInfo[Index]);
		}

		public void LoadImm(short Value)
		{
			ILGenerator.Emit(OpCodes.Ldc_I8, (long)Value);
		}

		public void Call(Type ClassType, string MethodName)
		{
			ILGenerator.EmitCall(OpCodes.Call, ClassType.GetMethod(MethodName), null);
		}

		public void Emit(OpCode OpCode)
		{
			ILGenerator.Emit(OpCode);
		}

		public void Ret()
		{
			ILGenerator.Emit(OpCodes.Ret);
		}

		public Action<CpuThreadState> CreateDelegate()
		{
			Ret();
			return (Action<CpuThreadState>)DynamicMethod.CreateDelegate(typeof(Action<CpuThreadState>));
		}
	}
}
