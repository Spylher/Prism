namespace Prism.Application.Dtos;

public class MeResponse
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IEnumerable<string> Roles { get; set; } = [];

    public LicenseInfoResponse License { get; set; } = new();
}