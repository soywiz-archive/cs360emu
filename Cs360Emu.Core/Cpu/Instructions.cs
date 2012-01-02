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
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/add.html"/>
		static public InstructionType Add = new InstructionType(Name: "add", Encoding: "$31:D:A:B:OE:%266:Rc", Format: "%rD, %rA, %rB", Category: Category.Cpu, Description: "Add");

		/// <summary>
		/// Add Carrying
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/addc.html"/>
		static public InstructionType Addc = new InstructionType(Name: "addc", Encoding: "$31:D:A:B:OE:%10:Rc", Format: "%rD, %rA, %rB", Category: Category.Cpu, Description: "Add Carrying");

		/// <summary>
		/// Add Extended
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/adde.html"/>
		static public InstructionType Adde = new InstructionType(Name: "adde", Encoding: "$31:D:A:B:OE:%138:Rc", Format: "%rD, %rA, %rB", Category: Category.Cpu, Description: "Add Extended");

		/// <summary>
		/// Add Immediate
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/addis.html"/>
		static public InstructionType AddI = new InstructionType(Name: "addi", Encoding: "$14:D:A:IMM16", Format: "%rD, %rA, %SIMM", Category: Category.Cpu, Description: "Add Immediate");

		/// <summary>
		/// Add Immediate Shifted
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/addis.html"/>
		static public InstructionType AddIS = new InstructionType(Name: "addis", Encoding: "$15:D:A:IMM16", Format: "%rD, %rA, %SIMM", Category: Category.Cpu, Description: "Add Immediate Shifted");

		/*
		/// <summary>
		/// Mofe from Special-Purpose Register
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/mfspr.html"/>
		static public InstructionType Mfspr = new InstructionType(Name: "mfspr", Encoding: "$31:D:----------:&10,339:0", Format: "%rD", Category: Category.Cpu, Description: "Mofe from Special-Purpose Register");
		*/

		/// <summary>
		/// Move from Link Register
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/mfspr.html"/>
		static public InstructionType Mflr = new InstructionType(Name: "mflr", Encoding: "$31:D:&5,8:-----:&10,339:0", Format: "%rD", Category: Category.Cpu, Description: "Move from Link Register");

		/// <summary>
		/// Move to Link Register
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/mtspr.html"/>
		static public InstructionType Mtlr = new InstructionType(Name: "mtlr", Encoding: "$31:D:&5,8:-----:&10,467:0", Format: "%rD", Category: Category.Cpu, Description: "Move to Link Register");

		/// <summary>
		/// Branch Conditional To Link Register
		/// </summary>
		/// <see cref="http://www.pds.twi.tudelft.nl/vakken/in101/labcourse/instruction-set/bclr.html"/>
		static public InstructionType Blr = new InstructionType(Name: "blr", Encoding: "$19:1-1--:-----:00000:&10,16:-", Format: "", Category: Category.Cpu, Description: "Branch Conditional To Link Register");
	}
}
