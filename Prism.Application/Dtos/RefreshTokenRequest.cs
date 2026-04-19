namespace Prism.Application.Dtos;

public record RefreshTokenRequest(
    string RefreshToken,
    string DeviceFingerprint
);