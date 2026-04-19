using Microsoft.EntityFrameworkCore;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;

namespace Prism.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context) => _context = context;

    public async Task<Session?> GetByRefreshTokenHashAsync(string refreshToken)
        => await _context.Sessions.FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshToken);

    public async Task<IEnumerable<Session>> GetActiveByClientIdAsync(Guid clientId)
        => await _context.Sessions
            .Where(s => s.ClientId == clientId && s.RevokedAt == null && s.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

    public async Task<IEnumerable<Session>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Sessions
            .Where(s => s.ClientId == clientId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Session session)
        => await _context.Sessions.AddAsync(session);

    public void Remove(Session session)
        => _context.Sessions.Remove(session);


}