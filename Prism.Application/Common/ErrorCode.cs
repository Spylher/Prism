namespace Prism.Application.Common;

public enum ErrorCode : uint
{
    None = 0,
    NotFound = 404,

    // Auth
    Unauthorized = 1000,
    Forbidden = 1001,
    InvalidCredentials = 1002,
    PasswordTooWeak = 1003,
    EmailAlreadyInUse = 1004,

    // Validation / Business
    ValidationError = 2000,
    InvalidEmail = 2001,

    // Infrastructure / Technical
    InfrastructureError = 500,
    UnexpectedError = 9000,
    Conflict = 409,

    // Domain
    ClientNotFound = 3000,
    ClientInactive = 3001,

    // Fallback
    Unknown = 400,
}
