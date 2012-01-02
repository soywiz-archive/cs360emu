using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Hle
{
	public enum ExportType
	{
		Variable,
		Function,
	}

	public class ExportAttribute : Attribute
	{
		public ushort Id;
		public ExportType Type;
	}
}
