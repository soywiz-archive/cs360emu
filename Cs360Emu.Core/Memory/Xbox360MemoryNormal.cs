using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;

namespace Cs360Emu.Core.Memory
{
	unsafe public sealed class Xbox360MemoryNormal : Xbox360MemoryAbstract
	{
		//Stream TempFileStream;
		byte* MainMemory;

		public Xbox360MemoryNormal()
		{
			MainMemory = (byte *)Marshal.AllocHGlobal(512 * 1024 * 1024).ToPointer();
		}

		public byte* GetPointer(ulong Address)
		{
			if (Address >= 0x80000000) return &MainMemory[Address - 0x80000000];
			throw (new InvalidOperationException(String.Format("Invalid address {0:X}", Address)));
		}

		public override void ReadBytes(ulong Address, byte* OutputData, int Length)
		{
			//Console.WriteLine("ReadBytes: {0:X}, {1}", Address, Length);
			PointerUtils.Memcpy(OutputData, GetPointer(Address), Length);
		}

		public override void WriteBytes(ulong Address, byte* InputData, int Length)
		{
			PointerUtils.Memcpy(GetPointer(Address), InputData, Length);
			/*
			Console.WriteLine("WriteBytes: {0:X}, {1}", Address, Length);
			if (Address >= 0x82000000)
			{
				//TempFileStream.Position = (long)(Address - 0x82000000);
				//TempFileStream.Write(PointerUtils.PointerToByteArray(InputData, Length), 0, Length);
			}
			else
			{
				throw(new InvalidOperationException(String.Format("Invalid address {0:X}", Address)));
			}
			*/
			//0x82000000
		}
	}
}
