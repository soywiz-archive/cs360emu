using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Cpu
{
	public partial class Instructions
	{
		public enum Category
		{
			Cpu = 1,
		}

		public sealed class InstructionType
		{
			public String Name { get; private set; }
			public String Encoding { get; private set; }
			public String Format { get; private set; }
			public String Description { get; private set; }
			public Category Category;

			public uint Mask { get; private set; }
			public uint Value { get; private set; }

			public bool hasOE { get; private set; }
			public bool hasRc { get; private set; }

			public InstructionType(String Name, String Encoding, String Format, String Description, Category Category)
			{
				this.Name = Name;
				this.Encoding = Encoding;
				this.Format = Format;
				this.Encoding = Encoding;this.Description = Description;
				this.Category = Category;
				ParseFormat(Encoding);
			}

			private void ParseFormat(String Format)
			{
				int Offset = 0;

				foreach (var Chunk in Format.Split(':'))
				{
					if (Chunk[0] == '$')
					{
						var ChunkValue = uint.Parse(Chunk.Substring(1));
						Value <<= 6;
						Mask <<= 6;
						Offset += 6;

						//Console.WriteLine("{0}", Value);

						Value |= ChunkValue;
						Mask |= (1 << 6) - 1;
						continue;
					}
		
					if (Chunk[0] == '%')
					{
						var ChunkValue = uint.Parse(Chunk.Substring(1));
						Value <<= 9;
						Mask <<= 9;
						Offset += 9;

						//Console.WriteLine("{0}", Value);

						Value |= ChunkValue;
						Mask |= (1 << 9) - 1;
						continue;
					}

					if (Chunk[0] == '&')
					{
						var Parts = Chunk.Substring(1).Split(',');
						var ChunkSize = int.Parse(Parts[0]);
						var ChunkValue = uint.Parse(Parts[1]);
						Value <<= ChunkSize;
						Mask <<= ChunkSize;
						Offset += ChunkSize;

						//Console.WriteLine("{0}", Value);

						Value |= ChunkValue;
						Mask |= (uint)((1 << ChunkSize) - 1);
						continue;
					}

					if (Chunk[0] == '0' || Chunk[0] == '1' || Chunk[0] == '-')
					{
						foreach (var Bit in Chunk)
						{
							Value <<= 1;
							Mask <<= 1;
							Offset += 1;
							switch (Bit)
							{
								case '0': Value |= 0; Mask |= 1; break;
								case '1': Value |= 1; Mask |= 1; break;
								case '-': Value |= 0; Mask |= 0; break;
								default: throw (new InvalidOperationException("Invalid chunk : " + Chunk));
							}
						}
						continue;
					}

					switch (Chunk)
					{
						case "OE":
						case "Rc":
							Value <<= 1;
							Mask <<= 1;
							Offset += 1;

							if (Chunk == "OE") hasOE = true;
							if (Chunk == "Rc") hasRc = true;

							break;
						case "A":
						case "B":
						case "D":
							Value <<= 5;
							Mask <<= 5;
							Offset += 5;
							break;
						case "IMM16":
							Value <<= 16;
							Mask <<= 16;
							Offset += 16;
							break;
						default:
							throw (new NotImplementedException("Can't handle type '" + Chunk + "'"));
					}
				}

				if (Offset != 32) throw (new InvalidOperationException("Invalid instruction : " + Offset));
			}
		}

		static private InstructionType[] _AllInstructions;

		static public InstructionType[] AllInstructions
		{
			get
			{
				if (_AllInstructions == null)
				{
					var Instructions = new List<InstructionType>();
					foreach (var Field in typeof(Instructions).GetFields())
					{
						Object Value = null;
						try
						{
							Value = Field.GetValue(null);
						}
						catch (Exception Exception)
						{
							Console.Error.WriteLine(Exception);
						}
						if (Value is Instructions.InstructionType)
						{
							Instructions.Add((Instructions.InstructionType)Value);
						}
					}
					_AllInstructions = Instructions.ToArray();
				}

				return _AllInstructions;
			}
		}
	}
}
