using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using QNetZ;

public class UbiservicesTokenFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Missing token"
            });
            return;
        }

        if (!QConfiguration.Instance.Ubiservices.Token.IsNullOrEmpty() && token != QConfiguration.Instance.Ubiservices.Token)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Invalid token"
            });
            return;
        }

        await next();
    }
}