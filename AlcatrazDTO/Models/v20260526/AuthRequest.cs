using System.ComponentModel.DataAnnotations;

namespace Alcatraz.DTO.Models.v20260526
{
    public class LoginRequest
    {
		[Required]
        public string email { get; set; }
		[Required]
        public string password { get; set; }
    }
    
    public class RegisterRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
		
        [Required]
		[MaxLength(256, ErrorMessage = "Password can't be longer than 256 characters.")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string password { get; set; }
        
		[Required]
		[MinLength(2, ErrorMessage = "Nickname must be at least 2 characters")]
		[MaxLength(14, ErrorMessage = "Nickname can't be longer than 14 characters (sorry)")]
        public string username { get; set; }
    }
}