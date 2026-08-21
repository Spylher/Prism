using Microsoft.EntityFrameworkCore;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
namespace Prism.Infrastructure.Repositories;

internal class AppProfileRepository(AppDbContext context) : RepositoryBase<AppProfile>(context), IAppProfileRepository
{
    private readonly AppDbContext _contextDb = context;

    public async Task<List<AppProfile>> GetByClientIdAsync(Guid clientId)
    {
        return await _contextDb.AppProfiles
            .Where(s => s.ClientId == clientId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

}
