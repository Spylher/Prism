using Microsoft.EntityFrameworkCore;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;

namespace Prism.Infrastructure.Repositories;

public class PlayerTagRepository : IPlayerTagRepository
{
    private readonly AppDbContext _context;

    public PlayerTagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerTag?> FindAsync(string playerNameNormalized)
    {
        return await _context.PlayerTags
            .FirstOrDefaultAsync(x =>
                x.PlayerNameNormalized == playerNameNormalized);
    }

    public async Task<List<PlayerTag>> GetOnlineByMapAsync(string mapName)
    {
        var timeout = DateTime.UtcNow.AddSeconds(-15);

        return await _context.PlayerTags
            .Where(x =>
                x.MapName == mapName &&
                x.LastSeenUtc >= timeout)
            .ToListAsync();
    }

    public async Task AddAsync(PlayerTag tag)
    {
        await _context.PlayerTags.AddAsync(tag);
    }

    public void Update(PlayerTag tag)
    {
        _context.PlayerTags.Update(tag);
    }
}