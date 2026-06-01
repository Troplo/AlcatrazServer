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
			return string.Format(
				"{0:X2}{1:X2}{2:X2}{3:X2}-" +
				"{4:X2}{5:X2}-" +
				"{6:X2}{7:X2}-" +
				"{8:X2}{9:X2}-" +
				"{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}",
				Bytes[0], Bytes[1], Bytes[2], Bytes[3],
				Bytes[4], Bytes[5],
				Bytes[6], Bytes[7],
				Bytes[8], Bytes[9],
				Bytes[10], Bytes[11], Bytes[12], Bytes[13], Bytes[14], Bytes[15]);
		}
	}
}
