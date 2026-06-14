using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Alcatraz.GameServices.Helpers
{
    public static class ApiErrorHelper
    {
        public static IActionResult CreateErrorResult(HttpContext context, TNTMPErrorCode code, string message, int statusCode = 400)
        {
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
                return new ObjectResult(new { error = message }) { StatusCode = statusCode };
            }

            var apiResponse = new ApiResponse<object>
            {
                Error = new ApiError
                {
                    Code = code,
                    DevMessage = message
                }
            };

            return new ObjectResult(apiResponse) { StatusCode = statusCode };
        }
    }
}
