using Prism.Domain.Entities;

namespace Prism.Domain.Interfaces;

public interface IPlayerTagRepository
{
    Task<PlayerTag?> FindAsync(string playerNameNormalized);
    Task<List<PlayerTag>> GetOnlineByMapAsync(string mapName);
    Task AddAsync(PlayerTag tag);
    void Update(PlayerTag tag);
}