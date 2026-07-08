using System.Linq;
using System.Threading.Tasks;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Alcatraz.GameServices.Helpers
{
    public class ApiVersionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int[] _supportedVersions =
        {
            // ubiservices
            1, 2, 3, 4, 5,
            // tntmp
            20260526, 20260614, 20260701
        };

        public ApiVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.GetRouteValue("version") is string versionStr)
            {
                int.TryParse(versionStr, out int version);
                if (_supportedVersions.Contains(version))
                {
                    context.Items["ApiVersion"] = version;
                    await _next(context);
                    return;
                }

                var errorResponse = new ApiResponse<object>
                {
                    Error = new ApiError
                    {
                        Code = TNTMPErrorCode.SERVER_VersionNotSupported,
                        DevMessage = "Version not supported by server."
                    }
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            await _next(context);
        }
    }
}
