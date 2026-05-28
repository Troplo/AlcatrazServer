using System.Collections.Generic;

namespace Alcatraz.DTO.Models.v20260526
{
	public class SessionBrowserItem
	{
		public uint id { get; set; }
		public uint typeID { get; set; }
		public uint hostPID { get; set; }
		public List<string> hostURLs { get; set; }
		public Dictionary<uint, uint> attributes { get; set; }
		public List<uint> publicParticipants { get; set; }
		public List<uint> privateParticipants { get; set; }
		public string ownerNickname { get; set; }
	}
}
