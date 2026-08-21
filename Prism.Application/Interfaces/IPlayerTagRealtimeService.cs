using Prism.Application.Dtos;

namespace Prism.Application.Interfaces;

public interface IPlayerTagRealtimeService
{
    Task<PlayerTagDto> UpsertAsync(Guid userId, PlayerLocationUpsertRequest request, DateTime nowUtc);
    Task<List<PlayerTagDto>> GetMapSnapshotAsync(string mapName);
    Task TouchAsync(string playerName, DateTime nowUtc); }