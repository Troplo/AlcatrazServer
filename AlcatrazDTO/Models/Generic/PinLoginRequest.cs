using System.ComponentModel.DataAnnotations;

namespace Alcatraz.DTO.Models.v20260526
{
	public class PinLoginRequest
	{
		[Required]
		[StringLength(8, MinimumLength = 8)]
		public string pin { get; set; }
	}
}
