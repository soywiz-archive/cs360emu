using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils;
using CSharpUtils.Endian;
using CSharpUtils.Extensions;

namespace Cs360Emu.Hle.Formats
{
	/// <summary>
	/// 
	/// </summary>
	/// <see cref="http://www.free60.org/XEX"/>
	unsafe public partial class Xex
	{
		public enum SectionTypeEnum : uint
		{
			Discardable = 0x02000000,
			NotCached = 0x04000000,
			NotPaged = 0x08000000,
			Shared = 0x10000000,
			Executable = 0x20000000,
			Readable = 0x40000000,
			Writable = 0x80000000,
		}

		public enum MediaTypes
		{
			HardDisk = 0x00000001,
			DvdX2 = 0x00000002,
			DvdCd = 0x00000004,
			Dvd5 = 0x00000008,
			Dvd9 = 0x00000010,
			SystemFlash = 0x00000020,
			MemoryUnit = 0x00000080,
			MassStorageDevice = 0x00000100,
			SmbFileSystem = 0x00000200,
			DirectFromRam = 0x00000400,
			InsecurePackage = 0x01000000,
			SaveGamePackage = 0x02000000,
			LocallySignedPackage = 0x04000000,
			LiveSignedPackage = 0x08000000,
			XboxPlatformPackage = 0x10000000,
		}

		public enum ModuleFlagsEnum
		{
			TitleModule = 1 << 0,
			ExportsToTitle = 1 << 1,
			SystemDebugger = 1 << 2,
			DllModule = 1 << 3,
			ModulePatch = 1 << 4,
			PatchFull = 1 << 5,
			PatchDelta = 1 << 6,
			UserMode = 1 << 7,
		}

		public struct HeaderStruct
		{
			/// <summary>
			/// 000 - XEX2
			/// </summary>
			public uint_be Magic;

			/// <summary>
			/// 004 - Maybe the file version?
			/// </summary>
			private uint_be _ModuleFlags;

			public ModuleFlagsEnum ModuleFlags()
			{
				return (ModuleFlagsEnum)(uint)_ModuleFlags;
			}

			/// <summary>
			/// 008 - ExecutablePointer (Starts a MZ executable)
			/// </summary>
			public uint_be PeDataOffset;

			/// <summary>
			/// 00C - ??
			/// </summary>
			public uint_be Reserved;

			/// <summary>
			/// 010 - Header Size?
			/// </summary>
			public uint_be SecurityInfoOffset;

			/// <summary>
			/// 014 -
			/// </summary>
			public uint_be OptionalHeaderCount;
		}

		public struct OptionalHeader
		{
			public enum Ids : uint
			{
				ResourceInfo = 0x2FF,
				BaseFileFormat = 0x3FF,
				BaseReference = 0x405,
				DeltaPatchDescriptor = 0x5FF,
				BoundingPath = 0x80FF,
				DeviceID = 0x8105,
				OriginalBaseAddress = 0x10001,
				EntryPoint = 0x10100,
				ImageBaseAddress = 0x10201,
				ImportLibraries = 0x103FF,
				ChecksumTimestamp = 0x18002,
				EnabledForCallcap = 0x18102,
				EnabledForFastcap = 0x18200,
				OriginalPEName = 0x183FF,	 
				StaticLibraries = 0x200FF,
				TLSInfo = 0x20104,	 
				DefaultStackSize = 0x20200,	 
				DefaultFilesystemCacheSize = 0x20301,	 
				DefaultHeapSize = 0x20401,	 
				PageHeapSizeAndFlags = 0x28002,	 
				SystemFlags = 0x30000,	 
				ExecutionID = 0x40006,	 
				ServiceIDList = 0x401FF,
				TitleWorkspaceSize = 0x40201,	 
				GameRatings = 0x40310,	 
				LANKey = 0x40404,	 
				Xbox360Logo = 0x405FF,	 
				MultidiscMediaIDs = 0x406FF,	 
				AlternateTitleIDs = 0x407FF,
				AdditionalTitleMemory = 0x40801,
				ExportsByName = 0xE10402,
			}

			private uint_be _Id;
			public Ids Id
			{
				set
				{
					_Id = (uint)value;
				}
				get
				{
					return (Ids)(uint)_Id;
				}
			}
			public uint_be Data;
			//public ulong_be Data;
		}

		public struct LibVersion
		{
			public ushort_be Version;
			public ushort_be Major;
			public ushort_be Minor;
			public ushort_be Patch;

			public override string ToString()
			{
				return String.Format("{0}.{1}.{2}.{3}", Version, Major, Minor, Patch);
			}
		}

		public struct GameRatingsStruct
		{
			public byte ESRB;
			public byte PEGI;
			public byte PEGI_FI;
			public byte PEGI_PT;
			public byte PEGI_BBFC;
			public byte CERO;
			public byte USK;
			public byte OFLCAU;
			public byte OFLCNZ;
			public byte KMRB;
			public byte BRASIL;
			public byte FPB;
		}

		public struct ChecksumTimestampStruct
		{
			public uint_be Checksum;
			public uint_be Timestamp;
		}

		public struct StaticLib
		{
			private fixed byte _RawName[8];
			public LibVersion Version;

			public String Name
			{
				get
				{
					fixed (byte* RawName = _RawName)
					{
						return PointerUtils.PtrToString(RawName, Encoding.UTF8);
					}
				}
			}
		}

		public HeaderStruct Header;
		public Stream PeStream;
		public Pe Pe;
		public Dictionary<OptionalHeader.Ids, ulong> InfoList;
		public String OriginalPeName;
		public byte[] LanKey;
		public uint DefaultStackSize;
		public ChecksumTimestampStruct ChecksumTimestamp;
		public uint ImageBaseAddress;
		public uint EntryPoint;
		public StaticLib[] StaticLibs;
		public TLSInfoStruct TLSInfo;

		/// <summary>
		/// Each thread will have that virtual memory address mapped to a distinct physical address.
		/// ? To confirm.
		/// </summary>
		public struct TLSInfoStruct
		{
			public uint_be NumberOfSlots;
			public uint_be DataSize;
			public uint_be RawDataAddress;
			public uint_be RawDataSize;
		}

		public Stream LoadChunk(Stream Stream, uint Offset)
		{
			return ReadChunkIncludingTheSize(Stream.SliceWithLength(Offset));
		}

		public Stream ReadChunk(Stream ChunkStream, bool IncludeTheSize = false)
		{
			var ChunkLength = (int)(uint)ChunkStream.ReadStruct<uint_be>();
			return ChunkStream.ReadStream(ChunkLength - (IncludeTheSize ? 4 : 0));
		}

		public Stream ReadChunkIncludingTheSize(Stream ChunkStream)
		{
			return ReadChunk(ChunkStream, IncludeTheSize: true);
		}

		public Xex LoadHeader(Stream XexStream)
		{
			Header = XexStream.ReadStruct<HeaderStruct>();
			var OptionalHeaders = XexStream.ReadStructVector<OptionalHeader>(Header.OptionalHeaderCount);
			Console.WriteLine("{0:X}", XexStream.Position);
			InfoList = new Dictionary<OptionalHeader.Ids, ulong>();
			foreach (var OptionalHeader in OptionalHeaders)
			{
				//Console.WriteLine("{0}: 0x{1:X}", OptionalHeader.Id, (uint)OptionalHeader.Data);
				//InfoList[OptionalHeader.Id] = OptionalHeader.Data;
				InfoList.Add(OptionalHeader.Id, OptionalHeader.Data);

				switch (OptionalHeader.Id)
				{
					case Xex.OptionalHeader.Ids.OriginalPEName:
						this.OriginalPeName = LoadChunk(XexStream, OptionalHeader.Data).ReadStringz(Encoding: Encoding.UTF8);
						break;
					case Xex.OptionalHeader.Ids.LANKey:
						this.LanKey = XexStream.SliceWithLength(OptionalHeader.Data, 0x10).ReadAll();
						break;
					case Xex.OptionalHeader.Ids.DefaultStackSize:
						this.DefaultStackSize = OptionalHeader.Data;
						break;
					case Xex.OptionalHeader.Ids.ChecksumTimestamp:
						this.ChecksumTimestamp = XexStream.SliceWithLength(OptionalHeader.Data).ReadStruct<ChecksumTimestampStruct>();
						break;
					case Xex.OptionalHeader.Ids.ImageBaseAddress:
						this.ImageBaseAddress = OptionalHeader.Data;
						break;
					case Xex.OptionalHeader.Ids.EntryPoint:
						this.EntryPoint = OptionalHeader.Data;
						break;
					case Xex.OptionalHeader.Ids.StaticLibraries:
						this.StaticLibs = LoadChunk(XexStream, OptionalHeader.Data).ReadStructVectorUntilTheEndOfStream<StaticLib>();
						foreach (var StaticLib in StaticLibs)
						{
							Console.WriteLine("StaticLib: {0}", StaticLib.ToStringDefault());
						}
						break;
					case Xex.OptionalHeader.Ids.ImportLibraries:
						{
							var ImportLibrariesStream = LoadChunk(XexStream, OptionalHeader.Data);
							var TextLength = (uint)ImportLibrariesStream.ReadStruct<uint_be>();
							var LibraryCount = (uint)ImportLibrariesStream.ReadStruct<uint_be>();
							var TextStream = ImportLibrariesStream.ReadStream(TextLength);

							var LibraryNames = new String[LibraryCount];
							for (int n = 0; n < LibraryCount; n++)
							{
								LibraryNames[n] = TextStream.ReadStringz(AllowEndOfStream: false);
								Console.WriteLine("ImportLib: {0}", LibraryNames[n]);
							}

							var ChunkUnk1 = ReadChunkIncludingTheSize(ImportLibrariesStream);
							var ImportAddressList = ReadChunkIncludingTheSize(ImportLibrariesStream);
							Console.Error.WriteLine("@TODO: Xex.OptionalHeader.Ids.ImportLibraries");
						}
						break;
					case Xex.OptionalHeader.Ids.TLSInfo:
						{
							this.TLSInfo = XexStream.SliceWithLength(OptionalHeader.Data).ReadStruct<TLSInfoStruct>();
						}
						break;
					default:
						Console.WriteLine("{0}: 0x{1:X}", OptionalHeader.Id, (uint)OptionalHeader.Data);
						break;
				}
			}
			PeStream = XexStream.SliceWithLength(Header.PeDataOffset);
			Pe = new Pe().LoadHeader(PeStream);

			Console.WriteLine("SecurityInfoOffset: {0:X}", (uint)Header.SecurityInfoOffset);
			var SecurityStream = LoadChunk(XexStream, Header.SecurityInfoOffset);
			var NumberOfSections = (ushort)SecurityStream.ReadStruct<ushort_be>();
			Console.WriteLine("NumberOfSections: {0:X}", NumberOfSections);

			return this;
		}
	}
}
