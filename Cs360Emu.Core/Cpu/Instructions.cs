using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu
{
	public partial class Instructions
	{
		/// <summary>
		/// Add
		/// </summary>
		static public InstructionType Add = new InstructionType(Encoding: "$31:D:A:B:OE:%266:Rc", Format: "%rD, %rA, %rB", Category: Category.Cpu, Description: "Add");

		/// <summary>
		/// Add Immediate Shifted
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/addis.html"/>
		static public InstructionType AddIS = new InstructionType(Encoding: "$15:D:A:IMM16", Format: "%rD, %rA, %SIMM", Category: Category.Cpu, Description: "Add Immediate Shifted");
	}
}
