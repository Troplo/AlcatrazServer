using System;
using System.IO;

namespace QNetZ.DDL
{
	public class qUUID : IAnyData
	{
		public byte[] Bytes = new byte[16];

		public qUUID() { }

		public qUUID(byte[] bytes)
		{
			if (bytes.Length != 16) throw new ArgumentException("qUUID must be 16 bytes");
			Bytes = bytes;
		}

		public void Read(Stream s)
		{
			s.Read(Bytes, 0, 16);
		}

		public void Write(Stream s)
		{
			s.Write(Bytes, 0, 16);
		}

		public static qUUID FromPID(uint pid)
		{
			var q = new qUUID();
			// Put PID at the end (similar to the hardcoded login UUID 0...1)
			q.Bytes[12] = (byte)((pid >> 24) & 0xFF);
			q.Bytes[13] = (byte)((pid >> 16) & 0xFF);
			q.Bytes[14] = (byte)((pid >> 8) & 0xFF);
			q.Bytes[15] = (byte)(pid & 0xFF);
			return q;
		}
		
		public override string ToString()
		{
			return new Guid(Bytes).ToString();
		}
	}
}
