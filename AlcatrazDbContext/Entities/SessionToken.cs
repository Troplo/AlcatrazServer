using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcatraz.Context.Entities
{
	public class SessionToken
	{
		[Key]
		public string Id { get; set; }
		
		public uint UserId { get; set; }
		
		public DateTime CreatedAt { get; set; }
		
		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}
}
