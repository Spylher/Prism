namespace Prism.Application.Dtos;

public record LoginClientResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string FullName
);