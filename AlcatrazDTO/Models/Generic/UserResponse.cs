using System;
using Alcatraz.DTO.Versioning;

namespace Alcatraz.DTO.Models.v20260526
{
	public class UserResponseV1
	{
		public uint id { get; set; }
		public Guid uuid { get; set; }
		public string email { get; set; }
		public string nickname { get; set; }
		
		[ApiVersionSince(20260614)]
		public int notorietyPoints { get; set; }
	}
}
