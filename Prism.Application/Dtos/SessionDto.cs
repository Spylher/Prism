namespace Prism.Application.Dtos;

public record SessionDto(
    Guid Id,
    string DeviceName,
    string DeviceFingerprint,
    string IpAddress,
    DateTime CreatedAt,
    DateTime ExpiresAt,
    DateTime? RevokedAt,
    string Status,
    string? RevocationReason
);