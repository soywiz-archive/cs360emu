using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu
{
	public sealed class CpuThreadState
	{
		/// <summary>
		/// General registers
		/// r0       - local      - commonly used to hold the old link register when building the stack frame
		/// r1       - dedicated  - stack pointer
		/// r2       - dedicated  - table of contents pointer
		/// r3       - local      - commonly used as the return value of a function, and also the first argument in
		/// r4–r10   - local      - commonly used to send in arguments 2 through 8 into a function
		/// r11–r12  - local      - general
		/// r13–r31  - global     - general
		/// </summary>
		public long r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14, r15, r16, r17, r18, r19, r20, r21, r22, r23, r24, r25, r26, r27, r28, r29, r30, r31;

		/// <summary>
		/// Link Register
		/// lr       - dedicated  - link register; cannot be used as a general register. Use mflr (move from link register) or mtlr (move to link register) to get at, e.g., mtlr r0
		/// </summary>
		public long lr;

		/// <summary>
		/// Conditional Register
		/// cr       - dedicated  - condition register
		/// </summary>
		public long cr;

		/// <summary>
		/// Program Counter
		/// </summary>
		public ulong pc;

		/// <summary>
		/// 
		/// </summary>
		public EmulatorState EmulatorState;
	}
}
