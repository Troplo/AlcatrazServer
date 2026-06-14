using System;

namespace Alcatraz.DTO.Models.v20260526
{
    public class ApiException : Exception
    {
        public TNTMPErrorCode ErrorCode { get; }
        public int? StatusCode { get; set; }

        public ApiException(TNTMPErrorCode code, string message) : base(message)
        {
            ErrorCode = code;
        }
    }
}
