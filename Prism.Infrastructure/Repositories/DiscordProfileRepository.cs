using Prism.Domain.Entities;
using Prism.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Prism.Domain.Interfaces;
namespace Prism.Infrastructure.Repositories;

internal class DiscordProfileRepository(AppDbContext context) : RepositoryBase<DiscordProfile>(context), IDiscordProfileRepository
{
    private readonly AppDbContext _contextDb = context;

    public async Task<List<DiscordProfile>> GetByClientIdAsync(Guid clientId)
    {
        return await _contextDb.DiscordProfiles
            .Where(s => s.ClientId == clientId && s.IsActive)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
}
