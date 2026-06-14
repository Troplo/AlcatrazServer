using Alcatraz.Context;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using Alcatraz.GameServices.Helpers;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v{version}/pins")]
	public class PinsController : ControllerBase
	{
		private readonly MainDbContext _dbContext;

		public PinsController(MainDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] PinLoginRequest request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.pin))
				return BadRequest("Invalid request.");

			var pinStr = request.pin.ToLowerInvariant();
			var pin = _dbContext.LoginPins.FirstOrDefault(p => p.Pin == pinStr);

			if (pin == null)
				return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.USER_InvalidPIN, "Invalid PIN", (int)HttpStatusCode.Unauthorized);

			if (pin.ExpiresAt < DateTime.UtcNow)
			{
				_dbContext.LoginPins.Remove(pin);
				_dbContext.SaveChanges();
				return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.USER_InvalidPIN, "PIN has expired", (int)HttpStatusCode.Unauthorized);
			}

			var token = pin.TokenId;

			_dbContext.LoginPins.Remove(pin);
			_dbContext.SaveChanges();

			return Ok(new SessionTokenResponse { token = token });
		}
	}
}
