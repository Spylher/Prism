using Microsoft.EntityFrameworkCore;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
namespace Prism.Infrastructure.Repositories;

public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Client>> GetActiveClientsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(client => client.IsActive)
            .ToListAsync();
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _dbSet
            .AnyAsync(client => client.Id == id);
    }
}
