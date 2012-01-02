using Cs360Emu.Hle.Formats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils.Extensions;
using Cs360Emu.Core.Memory;

namespace Cs360Emu.Tests
{
	[TestClass]
	public class XexTest
	{
		[TestMethod]
		public void LoadTest()
		{
			var Memory = new Xbox360MemoryNormal();
			var Xex = new Xex();
			Xex.LoadHeader(File.OpenRead("../../../TestInput/test.xex"));
			Xex.Pe.LoadSectionsTo(new Xbox360MemoryStream(Memory));
			Console.WriteLine(Xex.Header.ToStringDefault());
		}
	}
}
