using Prism.Domain.Common;

namespace Prism.Domain.Entities;

public class Session
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public string DeviceFingerprint { get; private set; }
    public string DeviceName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public SessionRevocationReason? RevocationReason { get; private set; }
    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
    public string IpAddress { get; private set; }
    public string WindowsUser { get; private set; }
    public string MacAddress { get; private set; }
    public string? LastIpAddress { get; private set; }
    protected Session() { }

    public Session(Guid clientId, string accessToken, string refreshTokenHash,
        string deviceFingerprint, string deviceName, string windowsUser, string macAddress, string ipAddress, DateTime expiresAt)
    {
        WindowsUser = windowsUser;
        MacAddress = macAddress;
        Id = Guid.NewGuid();
        ClientId = clientId;
        AccessToken = accessToken;
        RefreshTokenHash = refreshTokenHash;
        DeviceFingerprint = deviceFingerprint;
        DeviceName = deviceName;
        IpAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    public void UpdateLastIp(string ip) => LastIpAddress = ip;

    public bool IsDiscordSession(string discordKey) => discordKey == DeviceFingerprint;

    public void Revoke(SessionRevocationReason reason)
    {
        RevokedAt = DateTime.UtcNow;
        RevocationReason = reason;
    }

    public void RotateTokens(string newAccessToken, string refreshTokenHash, DateTime newExpiresAt)
    {
        AccessToken = newAccessToken;
        RefreshTokenHash = refreshTokenHash;
        ExpiresAt = newExpiresAt;
    }
}