namespace Prism.Application.Dtos;

public record ApplicationUserDto(
    Guid Id,
    Guid ClientId,
    string FullName,
    string Email
);