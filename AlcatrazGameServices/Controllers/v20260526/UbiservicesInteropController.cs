using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Alcatraz.Context;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QNetZ;

namespace Alcatraz.GameServices.Controllers.v20260526;

[ApiController]
[ServiceFilter(typeof(UbiservicesTokenFilter))]
[Route("api/v20260526/ubiservices")]
public class UbiservicesInteropController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly MainDbContext _dbContext;

    public UbiservicesInteropController(
        IUserService userService,
        MainDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }

    [HttpGet("user/get")]
    public IActionResult GetUser([FromQuery] string ticket)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            return BadRequest(new
            {
                error = "Missing ticket"
            });
        }

        var session = _dbContext.SessionTokens
            .FirstOrDefault(x => x.Id == ticket);

        if (session == null)
        {
            return Unauthorized(new
            {
                error = "Invalid ticket"
            });
        }

        UserModel user =
            _userService.GetById(session.UserId);

        if (user == null)
        {
            return Unauthorized(new
            {
                error = "User not found"
            });
        }

        return Ok(new
        {
            ticket = session.Id,
            userId = user.Id,
            profileId = user.Id.ToString(),
            nameOnPlatform = user.PlayerNickName,
            username = user.Username
        });
    }
}