using Prism.Domain.Entities;
namespace Prism.Domain.Interfaces;

public interface IClientRepository : IRepositoryBase<Client>
{
    Task<List<Client>> GetActiveClientsAsync();
    Task<bool> Exists(Guid id);
}
