using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Alcatraz.Context;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QNetZ;
using RDVServices;

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
    public IActionResult GetUser(
        [FromQuery] string? ticket,
        [FromQuery] Guid? guid)
    {
        if (string.IsNullOrWhiteSpace(ticket) && guid == null)
        {
            return BadRequest(new
            {
                error = "Either ticket or userId must be provided"
            });
        }
        
        UserModel user;

        if (!string.IsNullOrWhiteSpace(ticket))
        {
            var session = _dbContext.SessionTokens
                .FirstOrDefault(x => x.Id == ticket);

            
            if (session == null)
            {
                return Unauthorized(new
                {
                    error = "Invalid ticket"
                });
            }

            user =
                _userService.GetById(session.UserId);

            if (user == null)
            {
                return Unauthorized(new
                {
                    error = "User not found"
                });
            }
        }
        else if (guid != null)
        {
            user =
                _userService.GetByIdGuid(guid.Value);
            
            if (user == null)
            {
                return NotFound(new
                {
                    error = "User not found"
                });
            }
        }
        else
        {
            return NotFound(new
            {
                error = "User not found"
            });
        }
        
        return Ok(new
        {
            userId = user.Id,
            profileId = user.Id.ToString(),
            nameOnPlatform = user.PlayerNickName,
            username = user.Email,
            uuid = user.Guid
        });
    }
}