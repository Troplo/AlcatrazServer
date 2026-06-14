using Alcatraz.DTO.Models;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Alcatraz.GameServices.Helpers
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var user = (UserModel)context.HttpContext.Items["User"];
			if (user == null)
			{
				context.Result = ApiErrorHelper.CreateErrorResult(context.HttpContext, TNTMPErrorCode.SERVER_Unauthorized, "Unauthorized", 401);
			}
		}
	}
}
