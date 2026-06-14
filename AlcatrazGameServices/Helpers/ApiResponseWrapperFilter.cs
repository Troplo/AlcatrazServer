using System.Threading.Tasks;
using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QNetZ;

namespace Alcatraz.GameServices.Helpers
{
    public class ApiResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.HttpContext.Items.TryGetValue("ApiVersion", out var versionObj) && versionObj is int version)
            {
                // Initial release and ubiservices
                if (version == 20260526 || version == 1 || version == 2 || version == 3 || version == 4)
                {
                    await next();
                    return;
                }
            }

            if (context.Result is ObjectResult objectResult)
            {
                var type = objectResult.Value?.GetType();
                if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
                {
                    await next();
                    return;
                }

                int statusCode = objectResult.StatusCode ?? 200;

                if (statusCode >= 200 && statusCode < 300)
                {
                    var apiResponse = new ApiResponse<object>
                    {
                        Data = objectResult.Value
                    };
                    context.Result = new ObjectResult(apiResponse) { StatusCode = statusCode };
                }
                else
                {
                    var apiResponse = new ApiResponse<object>
                    {
                        Error = new ApiError
                        {
                            Code = MapStatusCodeToErrorCode(statusCode),
                            DevMessage = objectResult.Value?.ToString()
                        }
                    };
                    context.Result = new ObjectResult(apiResponse) { StatusCode = statusCode };
                }
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                int statusCode = statusCodeResult.StatusCode;
                if (statusCode >= 400)
                {
                    var apiResponse = new ApiResponse<object>
                    {
                        Error = new ApiError
                        {
                            Code = MapStatusCodeToErrorCode(statusCode),
                            DevMessage = null
                        }
                    };
                    context.Result = new ObjectResult(apiResponse) { StatusCode = statusCode };
                }
            }
            else if (context.Result is ContentResult contentResult)
            {
                int statusCode = contentResult.StatusCode ?? 200;
                if (statusCode >= 400)
                {
                    var apiResponse = new ApiResponse<object>
                    {
                        Error = new ApiError
                        {
                            Code = MapStatusCodeToErrorCode(statusCode),
                            DevMessage = contentResult.Content
                        }
                    };
                    context.Result = new ObjectResult(apiResponse) { StatusCode = statusCode };
                }
            }

            await next();
        }

        private TNTMPErrorCode MapStatusCodeToErrorCode(int statusCode)
        {
            return statusCode switch
            {
                400 => TNTMPErrorCode.SERVER_InvalidRequest,
                401 => TNTMPErrorCode.USER_InvalidCredentials,
                403 => TNTMPErrorCode.SERVER_Unauthorized,
                404 => TNTMPErrorCode.SERVER_NotFound,
                500 => TNTMPErrorCode.SERVER_InternalServerError,
                _ => TNTMPErrorCode.SERVER_UnknownError
            };
        }
    }
}
