namespace Prism.Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, Guid clientId, string fullName, string email, IList<string> roles);
    string GenerateRefreshToken();
    int RefreshTokenExpirationDays { get; }
}