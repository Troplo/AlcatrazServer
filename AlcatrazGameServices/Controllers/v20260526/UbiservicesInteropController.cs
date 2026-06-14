using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using Alcatraz.Context;
using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QNetZ;
using RDVServices;
using Alcatraz.GameServices.Helpers;
namespace Alcatraz.GameServices.Controllers.v20260526;

[ApiController]
[ServiceFilter(typeof(UbiservicesTokenFilter))]
[Route("api/v{version}/ubiservices")]
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
            return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.GenericValidationError, "Missing ticket.", (int)HttpStatusCode.BadRequest);
        }
        
        UserModel user;

        if (!string.IsNullOrWhiteSpace(ticket))
        {
            var session = _dbContext.SessionTokens
                .FirstOrDefault(x => x.Id == ticket);

            
            if (session == null)
            {
                return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session invalid or expired.", (int)HttpStatusCode.Unauthorized);
            }

            user =
                _userService.GetById(session.UserId);

            if (user == null)
            {
                return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session invalid or expired.", (int)HttpStatusCode.Unauthorized);
            }
        }
        else if (guid != null)
        {
            user =
                _userService.GetByIdGuid(guid.Value);
            
            if (user == null)
            {
                return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session invalid or expired.", (int)HttpStatusCode.Unauthorized);
            }
        }
        else
        {
            return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Session invalid or expired.", (int)HttpStatusCode.Unauthorized);
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