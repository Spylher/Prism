using Prism.Domain.Entities;

namespace Prism.Application.Dtos;

public record ClientProfileDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public ClientProfileDto(string name, string email, DateTime createdAt, bool isActive)
    {
        FullName = name;
        Email = email;
        CreatedAt = createdAt;
        IsActive = isActive;
    }

    public static ClientProfileDto FromDomain(UserReadModel user, Client client)
    {
        return new ClientProfileDto(user.UserName, user.Email, client.CreatedAt,client.IsActive);
    }
}
