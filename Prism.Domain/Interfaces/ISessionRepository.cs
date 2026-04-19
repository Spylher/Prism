using Prism.Domain.Entities;

namespace Prism.Domain.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByRefreshTokenHashAsync(string refreshToken);
    Task<IEnumerable<Session>> GetActiveByClientIdAsync(Guid clientId);
    Task<IEnumerable<Session>> GetByClientIdAsync(Guid clientId);
    Task AddAsync(Session session);
    void Remove(Session session);
}