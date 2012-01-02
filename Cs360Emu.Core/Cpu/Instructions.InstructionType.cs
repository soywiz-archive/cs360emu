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
			public String Encoding { get; private set; }
			public String Format { get; private set; }
			public String Description { get; private set; }
			public Category Category;

			public uint Mask { get; private set; }
			public uint Value { get; private set; }

			public bool hasOE;
			public bool hasRc;

			public InstructionType(String Encoding, String Format, String Description, Category Category)
			{
				this.Encoding = Encoding;
				this.Format = Format;
				this.Encoding = Encoding;this.Description = Description;
				this.Category = Category;
				ParseFormat(Encoding);
			}

			private void ParseFormat(String Format)
			{
				uint Offset = 0;

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
	}
}
