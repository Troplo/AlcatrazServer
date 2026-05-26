using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v20260526/user")]
	public class UserController : ControllerBase
	{
		[Authorize]
		[HttpGet("me")]
		public IActionResult GetMe()
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
				return Unauthorized();

			var response = new UserResponse
			{
				id = user.Id,
				username = user.Username,
				nickname = user.PlayerNickName
			};

			return Ok(response);
		}
	}
}
