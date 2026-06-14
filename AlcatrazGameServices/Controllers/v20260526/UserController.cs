using System.Net;
using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v{version}/user")]
	public class UserController : ControllerBase
	{
		private IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		
		[Authorize]
		[HttpGet("me")]
		public IActionResult GetMe()
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
			{ 
				return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session expired.", (int)HttpStatusCode.Unauthorized);
			}

			var response = new UserResponseV1
			{
				id = user.Id,
				uuid = user.Guid,
				email = user.Email,
				nickname = user.PlayerNickName,
				notorietyPoints = user.NotorietyPoints
			};

			return Ok(response);
		}

		[Authorize]
		[HttpPost("notoriety/sync")]
		public IActionResult SyncNotoriety([FromBody] NotorietySyncRequest request)
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
			{ 
				return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session expired.", (int)HttpStatusCode.Unauthorized);
			}
			
			int update = _userService.UpdateNotoriety(user.Guid, request.points, request.delta, request.sid, request.gm);

			var response = new NotorietySyncResponse()
			{
				points = update
			};

			return Ok(response);
		}
	}
}
