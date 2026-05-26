using Alcatraz.Context;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v20260526/pins")]
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
				return Unauthorized("Invalid PIN.");

			if (pin.ExpiresAt < DateTime.UtcNow)
			{
				_dbContext.LoginPins.Remove(pin);
				_dbContext.SaveChanges();
				return Unauthorized("PIN has expired.");
			}

			var token = pin.TokenId;

			_dbContext.LoginPins.Remove(pin);
			_dbContext.SaveChanges();

			return Ok(new SessionTokenResponse { token = token });
		}
	}
}
