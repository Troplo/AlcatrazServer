namespace Alcatraz.DTO.Models.v20260526
{
    public enum TNTMPErrorCode
    {
        None = 0,
        SERVER_UnknownError = 1,
        SERVER_InvalidRequest = 2,
        SERVER_Unauthorized = 3,
        SERVER_NotFound = 4,
        SERVER_InternalServerError = 5,
        SERVER_VersionNotSupported = 6,
        
        // USER
        USER_InvalidCredentials = 1000,
        USER_UsernameAlreadyExists = 1001,
        USER_EmailAlreadyExists = 1002,
        USER_InvalidPIN = 1003,
        USER_RegistrationsDisabled = 1004,
        GenericValidationError = 1005,
        USER_UserNotFound = 1006,
        USER_ClientUpdateRequired = 1007
    }
}