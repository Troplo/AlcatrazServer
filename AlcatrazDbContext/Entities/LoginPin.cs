using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcatraz.Context.Entities
{
	public class LoginPin
	{
		[Key]
		[StringLength(8)]
		[Column(TypeName = "varchar(8)")]
		public string Pin { get; set; }
		
		[Required]
		public string TokenId { get; set; }
		
		public DateTime ExpiresAt { get; set; }
		
		[ForeignKey("TokenId")]
		public virtual SessionToken Token { get; set; }
	}
}
