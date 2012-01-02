using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu.Disassembler
{
	public class Disassembler
	{
		public void Dissasemble()
		{
		}

		/// <summary>
		/// Very slow but simple implementation.
		/// </summary>
		/// <param name="InstructionValue"></param>
		/// <returns></returns>
		public static String DissasembleSimple(Instruction InstructionValue)
		{
			foreach (var InstructionType in Instructions.AllInstructions)
			{
				//Console.WriteLine("{0:X}, {1:X}, {2:X}", InstructionValue, InstructionType.Mask, InstructionType.Value);
				if ((InstructionValue & InstructionType.Mask) == InstructionType.Value)
				{
					var FieldName = InstructionType.Name;
					if (InstructionType.hasOE && InstructionValue.OE) FieldName += "o";
					if (InstructionType.hasRc && InstructionValue.Rc) FieldName += ".";
					var Result = String.Format("{0} {1}", FieldName, InstructionType.Format);
					Result = Result.Replace("%rA", "r" + InstructionValue.A);
					Result = Result.Replace("%rB", "r" + InstructionValue.B);
					Result = Result.Replace("%rD", "r" + InstructionValue.D);
					Result = Result.Replace("%SIMM", String.Format("0x{0:X}", InstructionValue.Imm16Unsigned));
					//InstructionValue.Ra

					return Result.Trim();
				}
			}
			return String.Format("<Unknown:0x{0:X}>", InstructionValue.Value);
		}
	}
}
