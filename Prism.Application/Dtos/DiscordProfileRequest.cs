namespace Prism.Application.Dtos;

public record DiscordProfileRequest(string UserId, string NickName, string GlobalName, string? AvatarHash);