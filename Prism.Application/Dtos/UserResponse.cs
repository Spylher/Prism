namespace Prism.Application.Dtos;

public record UserResponse(Guid UserId, string Email, string FullName, Guid ClientId, DateTime ExpiresAt, IList<string>? Roles);