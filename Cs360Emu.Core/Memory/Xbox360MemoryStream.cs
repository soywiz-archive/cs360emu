using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs360Emu.Core.Memory
{
	unsafe public class Xbox360MemoryStream : Stream
	{
		Xbox360MemoryNormal Xbox360Memory;

		public Xbox360MemoryStream(Xbox360MemoryNormal Xbox360Memory)
		{
			this.Xbox360Memory = Xbox360Memory;
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
		}

		public override long Length
		{
			get
			{
				return unchecked((1 << 63) - 1);
			}
		}

		private long _Position;

		public override long Position
		{
			get
			{
				return _Position;
			}
			set
			{
				_Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			fixed (byte* bufferStart = buffer)
			{
				Xbox360Memory.ReadBytes((ulong)_Position, &bufferStart[offset], count);
			}
			Position += count;
			return count;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin: _Position = offset; break;
				case SeekOrigin.Current: _Position += offset; break;
				case SeekOrigin.End: _Position = -offset; break;
			}
			return _Position;
		}

		public override void SetLength(long value)
		{
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			fixed (byte* bufferStart = buffer)
			{
				Xbox360Memory.WriteBytes((ulong)_Position, &bufferStart[offset], count);
			}
			Position += count;
		}
	}
}
