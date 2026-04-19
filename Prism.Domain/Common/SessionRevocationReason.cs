namespace Prism.Domain.Common;

public enum SessionRevocationReason
{
    None = 0,
    Logout,
    ReplacedByNewLogin,
    Expired,
    Security
}