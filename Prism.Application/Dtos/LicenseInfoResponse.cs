namespace Prism.Application.Dtos;

public class LicenseInfoResponse
{
    public DateTime ExpiresAt { get; set; }

    public bool IsExpired { get; set; }

    public int RemainingDays { get; set; }
}