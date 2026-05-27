using System;

namespace Alcatraz.DTO.Models.v20260526
{
	public class UserResponse
	{
		public uint id { get; set; }
		public Guid uuid { get; set; }
		public string email { get; set; }
		public string nickname { get; set; }
	}
}
