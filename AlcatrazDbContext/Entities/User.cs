using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System;

namespace Alcatraz.Context.Entities
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		
		public string Email { get; set; }
		public Guid Guid { get; set; } = Guid.NewGuid();
		public string PlayerNickName { get; set; }
		[JsonIgnore]
		public string Password { get; set; }
		public int RewardFlags { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public DateTime LastPlayTime { get; set; }
		public bool IsAdmin { get; set; }
	}
}
