using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;

namespace Prism.Application.Services;

public class SessionApplicationService : ISessionApplicationService
{
    private readonly ISessionRepository _sessionRepo;

    public SessionApplicationService(ISessionRepository sessionRepo)
    {
        _sessionRepo = sessionRepo;
    }

    public async Task<IEnumerable<SessionDto>> GetSessionsByClientIdAsync(Guid clientId)
    {
        var sessions = await _sessionRepo.GetByClientIdAsync(clientId);

        return sessions.Select(s => new SessionDto(
            s.Id,
            s.DeviceName,
            s.DeviceFingerprint,
            s.IpAddress,
            s.CreatedAt,
            s.ExpiresAt,
            s.RevokedAt,
            s.IsActive ? "Active" : "Inactive",
            s.RevocationReason?.ToString()
        ));
    }
}