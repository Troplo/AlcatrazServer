using System.Net;
using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alcatraz.GameServices.Controllers.v20260526
{
    [ApiController]
    [Route("api/v{version}/marketplace")]
    public class MarketplaceController : ControllerBase
    {
        private IUserService _userService;

        public MarketplaceController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost("createMarketplaceToken")]
        public IActionResult CreateMarketplaceToken()
        {
            var user = (UserModel)HttpContext.Items["User"];

            if (user == null)
            {
                return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.SERVER_Unauthorized,
                    "Session expired.", (int)HttpStatusCode.Unauthorized);
            }
            
            var jwtToken = _userService.CreateMarketplaceJwt(user.Guid);
			return Ok(jwtToken);
        }
    }
}