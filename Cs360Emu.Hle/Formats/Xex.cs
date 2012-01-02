using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils.Endian;

namespace Cs360Emu.Hle.Formats
{
	public class Xex
	{
		/// <summary>
		/// 00 - XEX2
		/// </summary>
		uint_be Magic;

		/// <summary>
		/// 04 - Maybe the file version?
		/// </summary>
		uint_be FileVersionUnknown;

		/// <summary>
		/// 08 - ExecutablePointer (Starts a MZ executable)
		/// </summary>
		uint_be ExecutablePointer;

		/// <summary>
		/// 0C - ??
		/// </summary>
		uint_be _Unknown_0C;

		/// <summary>
		/// 10 - Header Size?
		/// </summary>
		uint_be HeaderSize;

		/// <summary>
		/// 14 -
		/// </summary>
		uint_be _Unknown_14;

		/// <summary>
		/// 18 -
		/// </summary>
		uint_be _Unknown_18;

		/// <summary>
		/// 1C - PointerToStaticLibraryImports?
		/// </summary>
		uint_be PointerToStaticLibraryImports;

		/// <summary>
		/// 20 -
		/// </summary>
		uint_be _Unknown_20;

		/// <summary>
		/// 24 -
		/// </summary>
		uint_be _Unknown_24;

		/// <summary>
		/// 28 -
		/// </summary>
		uint_be _Unknown_28;

		/// <summary>
		/// 2C - LoadAddress - 0x_82_00_00_00
		/// </summary>
		uint_be LoadAddress;

		/// <summary>
		/// 30 -
		/// </summary>
		uint_be _Unknown_30;

		/// <summary>
		/// 34 - EntryPoint
		/// </summary>
		uint_be EntryPoint;

	}
}
