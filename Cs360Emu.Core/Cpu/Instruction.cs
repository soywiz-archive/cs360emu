using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;

namespace Cs360Emu.Core.Cpu
{
	public struct Instruction
	{
		public uint Value;

		public ushort Imm16Unsigned
		{
			get
			{
				return (ushort)((Value >> 0) & 0xFFFF);
			}
		}

		public short Imm16Signed
		{
			get
			{
				return (short)((Value >> 0) & 0xFFFF);
			}
		}

		public uint A
		{
			get
			{
				return BitUtils.Extract(Value, 16, 5);
			}
		}

		public uint B
		{
			get
			{
				return BitUtils.Extract(Value, 11, 5);
			}
		}

		public uint D
		{
			get
			{
				return BitUtils.Extract(Value, 21, 5);
			}
		}

		public uint S
		{
			get
			{
				return BitUtils.Extract(Value, 21, 5);
			}
		}

		/// <summary>
		/// This bit specify if the CR0 (Condition Register) will be updated.
		/// </summary>
		public bool Rc
		{
			get
			{
				return BitUtils.Extract(Value, 0, 1) != 0;
			}
		}

		public bool OE
		{
			get
			{
				return BitUtils.Extract(Value, 10, 1) != 0;
			}
		}

		public uint Type0
		{
			get
			{
				return BitUtils.Extract(Value, 26, 6);
			}
		}

		public uint Type1
		{
			get
			{
				return BitUtils.Extract(Value, 1, 9);
			}
		}

		public static implicit operator uint(Instruction that)
		{
			return that.Value;
		}

		public static implicit operator Instruction(uint that)
		{
			return new Instruction()
			{
				Value = that,
			};
		}
	}
}
