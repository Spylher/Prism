using Prism.Domain.Entities;
namespace Prism.Domain.Interfaces;

public interface IAppProfileRepository : IRepositoryBase<AppProfile>
{
    Task<List<AppProfile>> GetByClientIdAsync(Guid clientId);
}
