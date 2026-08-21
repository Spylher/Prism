namespace Prism.Domain.Entities;

public class AppProfile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid ClientId { get; set; }
}