using System;
using System.Threading.Tasks;
using Alcatraz.Context;
using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNetZ;

namespace Alcatraz.GameServices.Controllers.v20260526
{
    [ApiController]
    [Route("api/v20260526/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly MainDbContext _dbContext;
        
        public AuthController(IUserService userService, MainDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
                return BadRequest("Invalid request.");

            var res = _userService.Authenticate(
                new AuthenticateRequest
                {
                    Email = request.email,
                    Password = request.password
                });

            if (res == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = new Alcatraz.Context.Entities.SessionToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = res.Id,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.SessionTokens.Add(token);
            await _dbContext.SaveChangesAsync();

            return Ok(new SessionTokenResponse { token = token.Id });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password) || string.IsNullOrWhiteSpace(request.username))
                return BadRequest("Invalid request.");

            var result = _userService.Register(new UserRegisterModel
            {
                Email = request.email,
                Password = request.password,
                PlayerNickName = request.username
            });

            if (!result.Success)
                return BadRequest(result);

            var token = new Alcatraz.Context.Entities.SessionToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = result.PlayerId,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.SessionTokens.Add(token);
            await _dbContext.SaveChangesAsync();
            
            return Ok(new SessionTokenResponse { token = token.Id });
        }
    }
}