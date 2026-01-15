namespace Prism.Application.Dtos;

public record UserReadModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Guid ClientId { get; set; }

    public UserReadModel(Guid id, string userName, string email, Guid clientId)
    {
        Id = id;
        UserName = userName;
        Email = email;
        ClientId = clientId;
    }
}
