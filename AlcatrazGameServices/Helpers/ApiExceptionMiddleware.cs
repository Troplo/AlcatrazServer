using System.Threading.Tasks;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Alcatraz.GameServices.Helpers
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException apiEx)
            {
                await HandleApiExceptionAsync(context, apiEx);
            }
        }

        private async Task HandleApiExceptionAsync(HttpContext context, ApiException apiEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = apiEx.StatusCode ?? 400;

            int version = 0;
            if (context.Items.TryGetValue("ApiVersion", out var versionObj) && versionObj is int parsedVersionItem)
            {
                version = parsedVersionItem;
            }
            else if (context.GetRouteValue("version") is string versionStr && int.TryParse(versionStr, out int parsedVersionRoute))
            {
                version = parsedVersionRoute;
            }
            
            // Initial release and ubiservices
            if (version == 20260526 || version == 1 || version == 2 || version == 3 || version == 4)
            {
                // Old format
                await context.Response.WriteAsJsonAsync(new { error = apiEx.Message });
                return;
            }

            var apiResponse = new ApiResponse<object>
            {
                Error = new ApiError
                {
                    Code = apiEx.ErrorCode,
                    DevMessage = apiEx.Message
                }
            };

            await context.Response.WriteAsJsonAsync(apiResponse);
        }
    }
}
