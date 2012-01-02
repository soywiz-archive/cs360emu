using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils.Extensions;

namespace Cs360Emu.Hle.Formats
{
	unsafe public class DosExe
	{
		// DOS exe format
		public struct HeaderStruct
		{
			public ushort Signature; 
			public ushort LengthOfImage;  
			public ushort SizeOfFile; 
			public ushort NumberOfRelocationItems; 
			public ushort SizeOfHeader; 
			public ushort MinPara; 
			public ushort MaxPara; 
			public ushort OffsetStack; 
			public ushort InitialSp; 
			public ushort NegativeChecksum;
			public ushort InitialIp;
			public ushort OffsetCs;
			public ushort OffsetFirstRelocationItem;
			public ushort OverlayNumber; 
			public ushort Res1;
			public ushort Res2;
			public ushort Res3;
			public ushort Res4;
			public ushort OemId;       
			public ushort OemInfo;
			public uint   _Reserved0;
			public uint   _Reserved1;
			public uint   _Reserved2;
			public uint   _Reserved3;
			public uint   _Reserved4;
			public uint   OffsetToPEHeader;
		}

		public HeaderStruct Header;

		public DosExe LoadHeader(Stream ExeStream)
		{
			this.Header = ExeStream.ReadStruct<DosExe.HeaderStruct>();

			return this;
		}
	}
}
