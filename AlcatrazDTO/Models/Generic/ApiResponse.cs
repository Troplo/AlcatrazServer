#nullable enable
namespace Alcatraz.DTO.Models.v20260526
{
    public class ApiError
    {
        public TNTMPErrorCode Code { get; set; }
        public string? DevMessage { get; set; }
    }
    
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public ApiError? Error { get; set; }
        public bool Success => Error == null;
    }
}