using System.Collections.Generic;

namespace Alcatraz.DTO.Models.v20260526
{
	public class SessionBrowserItem
	{
		public uint Id { get; set; }
		public uint TypeID { get; set; }
		public uint HostPID { get; set; }
		public List<string> HostURLs { get; set; }
		public Dictionary<uint, uint> Attributes { get; set; }
		public List<uint> PublicParticipants { get; set; }
		public List<uint> PrivateParticipants { get; set; }
	}
}
