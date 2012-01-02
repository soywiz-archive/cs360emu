using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using CSharpUtils.Extensions;

namespace Cs360Emu.Hle.Formats
{
	unsafe public class Pe
	{
		public struct PeHeaderStruct
		{
			public enum MachineEnum : ushort
			{
				Xbox360 = 0x1F2,
			}

      		public uint  Signature;
			public MachineEnum Machine;	  
  			public ushort NumSections; 
			public uint   TimeDateStamp;
			public uint   PointerToSymbolTable;          
			public uint   NumberOfSymbols;    
			public ushort SizeOfOptionalHeader; 
			public ushort Characteristics;   
		}

		public struct DataDirStruct
		{
			public uint VirtualAddress;
			public uint Size;
		}

		public enum Subsystem
		{
			UNKNOWN = 0, NATIVE = 1, WINDOWS_GUI = 2,
			WINDOWS_CONSOLE = 3, OS2_CONSOLE = 5,
			POSIX_CONSOLE = 7, NATIVE_WIN9X_DRIVER = 8,
			WINDOWS_CE_GUI = 9, EFI_APPLICATION = 10,
			EFI_BOOT_SERVICE_DRIVER = 11, EFI_RUNTIME_DRIVER = 12,
			EFI_ROM = 13, XBOX = 14
		}

		public struct PeOptionalHeaderStruct
		{
			// Standard fields.
			public ushort Magic;
			public byte   MajorLinkerVersion;
			public byte   MinorLinkerVersion;
			public uint   SizeOfCode;
			public uint   SizeOfInitializedData;
			public uint   SizeOfUninitializedData;
			public uint   AddressOfEntryPoint;
			public uint   BaseOfCode;
			public uint   BaseOfData;
			// NT additional fields.
			public uint   ImageBase;
			public uint   SectionAlignment;
			public uint   FileAlignment;
			public ushort MajorOperatingSystemVersion;
			public ushort MinorOperatingSystemVersion;
			public ushort MajorImageVersion;
			public ushort MinorImageVersion;
			public ushort MajorSubsystemVersion;
			public ushort MinorSubsystemVersion;
			public uint   Win32VersionValue;
			public uint   SizeOfImage;
			public uint   SizeOfHeaders;
			public uint   CheckSum;
			public Subsystem Subsystem;
			public ushort DllCharacteristics;
			public uint   SizeOfStackReserve;
			public uint   SizeOfStackCommit;
			public uint   SizeOfHeapReserve;
			public uint   SizeOfHeapCommit;
			public uint   LoaderFlags;
			public uint   NumberOfRvaAndSizes;
			//public IMAGE_DATA_DIRECTORIES DataDirectory;

			public DataDirStruct Export;
			public DataDirStruct Import;
			public DataDirStruct Resource;
			public DataDirStruct Exception;
			public DataDirStruct Security;
			public DataDirStruct BaseRelocationTable;
			public DataDirStruct DebugDirectory;
			public DataDirStruct CopyrightOrArchitectureSpecificData;
			public DataDirStruct GlobalPtr;
			public DataDirStruct TLSDirectory;
			public DataDirStruct LoadConfigurationDirectory;
			public DataDirStruct BoundImportDirectory;
			public DataDirStruct ImportAddressTable;
			public DataDirStruct DelayLoadImportDescriptors;
			public DataDirStruct COMRuntimedescriptor;
			//public DataDirStruct Reserved;
		}

		public struct SectionFlagsStruct
		{
			public ushort Flags;

			public bool Code { get { return BitUtils.ExtractBool(Flags, 5); } }
			public bool InitializedData { get { return BitUtils.ExtractBool(Flags, 6); } }
			public bool UninitializedData { get { return BitUtils.ExtractBool(Flags, 7); } }
			public bool Reserved2 { get { return BitUtils.ExtractBool(Flags, 8); } }
			public bool LinkerInfoOrComments { get { return BitUtils.ExtractBool(Flags, 9); } }
			public bool Reserved3 { get { return BitUtils.ExtractBool(Flags, 10); } }
			public bool LinkerShouldRemove { get { return BitUtils.ExtractBool(Flags, 11); } }
			public bool CommonBlockData { get { return BitUtils.ExtractBool(Flags, 12); } }
			public bool Reserved4 { get { return BitUtils.ExtractBool(Flags, 13); } }
			public bool NoDeferSpeculativeExceptions { get { return BitUtils.ExtractBool(Flags, 14); } }
			public bool FarData { get { return BitUtils.ExtractBool(Flags, 15); } }
			public bool Reserved5 { get { return BitUtils.ExtractBool(Flags, 16); } }
			public bool PurgeableOr16Bit { get { return BitUtils.ExtractBool(Flags, 17); } }
			public bool Locked { get { return BitUtils.ExtractBool(Flags, 18); } }
			public bool PreLoad { get { return BitUtils.ExtractBool(Flags, 19); } }

			/*
			DWORD Reserved : 5;
				DWORD Code : 1;
				DWORD InitializedData: 1;
				DWORD UninitializedData : 1;
				DWORD Reserved2: 1;
				DWORD LinkerInfoOrComments: 1;
				DWORD Reserved3: 1;
				DWORD LinkerShouldRemove: 1;
				DWORD CommonBlockData: 1;
				DWORD Reserved4: 1;
				DWORD NoDeferSpeculativeExceptions: 1;
				DWORD FarData: 1;
				DWORD Reserved5: 1;
				DWORD PurgeableOr16Bit: 1;
				DWORD Locked: 1;
				DWORD PreLoad: 1;
			DWORD Alignment: 4;
			DWORD ExtendedRelocations: 1;
			DWORD Discardable: 1;
			DWORD NotCachable: 1;
			DWORD NotPageable: 1;
			DWORD Shareable: 1;
			DWORD Executable: 1;
			DWORD Readable: 1;
			DWORD Writeable: 1;
			*/

			public override string ToString()
			{
				return this.ToStringDefault(SimplifyBool: true);
				//return String.Format("{0}", Flags);
			}
		}

		public struct ImageSectionHeader
		{
			private fixed byte NameRaw[8];
			public uint   VirtualSize;
			public uint   VirtualAddress;
			public uint   SizeOfRawData;
			public uint   PointerToRawData;
			public uint   NonUsedPointerToRelocations;
			public uint   NonUsedPointerToLinenumbers;
			public ushort NonUsedNumberOfRelocations;
			public ushort NonUsedNumberOfLinenumbers;
			public SectionFlagsStruct Characteristics;
			public string Name
			{
				get
				{
					fixed (byte * NameRawPtr = NameRaw)
					{
						return PointerUtils.PtrToString(NameRawPtr, Encoding.UTF8);
					}
				}
			}
		}

		public Stream ExeStream;
		public DosExe DosExe;
		public Stream PeStream;
		public PeHeaderStruct PeHeader;
		public Stream OptionalHeaderStream;
		public PeOptionalHeaderStruct PeOptionalHeader;
		public ImageSectionHeader[] ImageSectionHeaderList;

		public Pe LoadHeader(Stream _ExeStream)
		{
			ExeStream = _ExeStream;
			DosExe = new DosExe().LoadHeader(ExeStream);
			PeStream = ExeStream.SliceWithLength(DosExe.Header.OffsetToPEHeader);
			PeHeader = PeStream.ReadStruct<PeHeaderStruct>();
			OptionalHeaderStream = PeStream.ReadStream(PeHeader.SizeOfOptionalHeader);
			PeOptionalHeader = OptionalHeaderStream.ReadStruct<PeOptionalHeaderStruct>();
			ImageSectionHeaderList = new ImageSectionHeader[PeHeader.NumSections];
			for (int n = 0; n < PeHeader.NumSections; n++)
			{
				ImageSectionHeaderList[n] = PeStream.ReadStruct<ImageSectionHeader>();
			}

			return this;
		}

		public void LoadSectionsTo(Stream MemoryStream)
		{
			Console.WriteLine("{0:X}", ExeStream.Length);

			//400-1600 -> 82000400-820015E4

			//PeStream.SliceWithLength(0).CopyToFast(MemoryStream.SliceWithLength(0x82000000));
			//return;

			//var BaseMemoryStream = MemoryStream.SliceWithLength(PeOptionalHeader.ImageBase);

			File.WriteAllBytes(@"C:\projects\csharp\cs360emu\test.bin", ExeStream.ReadAll());

			foreach (var ImageSectionHeader in ImageSectionHeaderList)
			{
				Console.WriteLine("-------------------------------------------------");
				Console.WriteLine(ImageSectionHeader.ToStringDefault());
				var FileSlice = ExeStream.SliceWithLength(ImageSectionHeader.PointerToRawData, ImageSectionHeader.SizeOfRawData);
				var MemorySlice = MemoryStream.SliceWithLength(ImageSectionHeader.VirtualAddress + PeOptionalHeader.ImageBase, ImageSectionHeader.VirtualSize);
				Console.WriteLine("Segment: {0:X}-{1:X} -> {2:X}-{3:X}", FileSlice.SliceLow, FileSlice.SliceHigh, MemorySlice.SliceLow, MemorySlice.SliceHigh);

				var Data = FileSlice.ReadAll(true);
				//Console.WriteLine(BitConverter.ToString(Data));
				MemorySlice.WriteBytes(Data);
				//FileSlice.Position = 0;
				//MemorySlice.Position = 0;
				//FileSlice.CopyToFast(MemorySlice);
			}
		}
	}
}
