
namespace Prism.Domain.Entities;

public class DiscordProfile
{
    public Guid Id { get; set; }
    public string DiscordUserId { get; set; } = string.Empty;
    public string? DiscordNickName { get; set; }
    public string? DiscordGlobalName { get; set; }
    public string? DiscordAvatarHash { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; private set; }
    public Guid ClientId { get; set; }
    public bool IsActive => RevokedAt is null;


    public void Revoke() => RevokedAt = DateTime.UtcNow;

}
