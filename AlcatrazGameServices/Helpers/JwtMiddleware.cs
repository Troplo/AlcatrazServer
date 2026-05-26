using Alcatraz.Context;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Helpers
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AppSettings _appSettings;

		public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
		{
			_next = next;
			_appSettings = appSettings.Value;
		}

		public async Task Invoke(HttpContext context, IUserService userService, Alcatraz.Context.MainDbContext dbContext)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (string.IsNullOrEmpty(token))
			{
				token = context.Request.Headers["x-tntmp-token"].FirstOrDefault();
			}

			if (!string.IsNullOrEmpty(token))
				attachUserToContext(context, userService, dbContext, token);

			await _next(context);
		}

		private void attachUserToContext(HttpContext context, IUserService userService, MainDbContext dbContext, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var nameId = jwtToken.Claims.FirstOrDefault(x => x.Type == "uid");
				var userId = uint.Parse(nameId.Value);

				context.Items["User"] = userService.GetById(userId);
			}
			catch
			{
				var sessionToken = dbContext.SessionTokens.FirstOrDefault(t => t.Id == token);
				if (sessionToken != null)
				{
					context.Items["User"] = userService.GetById(sessionToken.UserId);
				}
			}
		}
	}
}
