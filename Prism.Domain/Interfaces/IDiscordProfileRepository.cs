using Prism.Domain.Entities;

namespace Prism.Domain.Interfaces;

public interface IDiscordProfileRepository : IRepositoryBase<DiscordProfile>
{
    Task<List<DiscordProfile>> GetByClientIdAsync(Guid clientId);
}
