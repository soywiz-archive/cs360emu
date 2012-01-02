using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Memory
{
	unsafe public abstract class Xbox360MemoryAbstract
	{
		public abstract void ReadBytes(ulong Address, byte* OutputData, int Length);
		public abstract void WriteBytes(ulong Address, byte* InputData, int Length);

		public byte[] ReadBytes(ulong Address, int Length)
		{
			var Data = new byte[Length];
			fixed (byte* DataPtr = Data)
			{
				ReadBytes(Address, DataPtr, Length);
			}
			return Data;
		}

		public struct SegmentStruct
		{
			public SegmentStruct(ulong Start, ulong End)
			{
				this.Start = Start;
				this.End = End;
			}

			public bool Inside(ulong Address)
			{
				return (Address >= Start) && (Address < End);
			}

			public ulong Start;
			public ulong End;
		}

		public readonly SegmentStruct Virtual4KB    = new SegmentStruct(0x000000000, 0x040000000);
		public readonly SegmentStruct Virtual64KB   = new SegmentStruct(0x040000000, 0x080000000);
		public readonly SegmentStruct Image64KB     = new SegmentStruct(0x080000000, 0x08C000000);
		public readonly SegmentStruct Encrypted64KB = new SegmentStruct(0x08C000000, 0x08E000000);
		public readonly SegmentStruct Image4KB      = new SegmentStruct(0x090000000, 0x0A0000000);
		public readonly SegmentStruct Physical64KB  = new SegmentStruct(0x0A0000000, 0x0C0000000);
		public readonly SegmentStruct Physical16MB  = new SegmentStruct(0x0C0000000, 0x0E0000000);
		public readonly SegmentStruct Physical4KB   = new SegmentStruct(0x0E0000000, 0x100000000);

		public int GetPageSizeForAddress(ulong Address)
		{
			if (Virtual4KB.Inside(Address)) return 4 * 1024;
			if (Virtual64KB.Inside(Address)) return 64 * 1024;
			if (Image64KB.Inside(Address)) return 64 * 1024;
			if (Encrypted64KB.Inside(Address)) return 64 * 1024;
			if (Image4KB.Inside(Address)) return 4 * 1024;
			if (Physical64KB.Inside(Address)) return 64 * 1024;
			if (Physical16MB.Inside(Address)) return 16 * 1024 * 1024;
			if (Physical4KB.Inside(Address)) return 4 * 1024;
			throw(new InvalidOperationException());
		}

		public byte Read1(ulong Address)
		{
			var Data = stackalloc byte[1];
			ReadBytes(Address, Data, 1);
			return *(byte*)Data;
		}

		public ushort Read2(ulong Address)
		{
			var Data = stackalloc byte[2];
			ReadBytes(Address, Data, 2);
			return *(ushort*)Data;
		}

		public uint Read4(ulong Address)
		{
			var Data = stackalloc byte[4];
			ReadBytes(Address, Data, 4);
			return *(uint*)Data;
		}

		public ulong Read8(ulong Address)
		{
			var Data = stackalloc byte[8];
			ReadBytes(Address, Data, 8);
			return *(ulong*)Data;
		}
	}
}
