using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;

namespace Prism.Application.Services;

public class PlayerTagRealtimeService : IPlayerTagRealtimeService
{
    private readonly IPlayerTagRepository _repo;
    private readonly IUnitOfWork _uow;

    public PlayerTagRealtimeService(IPlayerTagRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PlayerTagDto> UpsertAsync(Guid userId, PlayerLocationUpsertRequest request, DateTime nowUtc)
    {
        var normalized =
            request.PlayerName.ToUpperInvariant();

        var player = await _repo.FindAsync(normalized);

        if (player is null)
        {
            player = new PlayerTag(
                userId,
                request.PlayerName,
                request.MapName,
                request.X,
                request.Y,
                request.GroupName,
                request.ClassId,
                request.Direction,
                nowUtc);

            await _repo.AddAsync(player);
        }
        else
        {
            player.Update(
                request.MapName,
                request.X,
                request.Y,
                request.GroupName,
                request.ClassId,
                request.Direction,
                nowUtc);

            _repo.Update(player);
        }

        await _uow.CommitAsync();

        return ToDto(player);
    }

    public async Task<List<PlayerTagDto>> GetMapSnapshotAsync(string mapName)
    {
        var players = await _repo.GetOnlineByMapAsync(mapName);

        return players
            .Select(ToDto)
            .ToList();
    }

    public async Task TouchAsync(string playerName, DateTime nowUtc)
    {
        var normalized =
            playerName.ToUpperInvariant();

        var player = await _repo.FindAsync(normalized);

        if (player is null)
            return;

        player.Touch(nowUtc);
        _repo.Update(player);
        await _uow.CommitAsync();
    }

    private static PlayerTagDto ToDto(PlayerTag player)
    {
        return new PlayerTagDto
        {
            Id = player.Id,
            OwnerUserId = player.OwnerUserId,
            PlayerName = player.PlayerName,
            MapName = player.MapName,
            X = player.X,
            Y = player.Y,
            GroupName = player.GroupName,
            ClassId = player.ClassId,
            Direction = player.Direction,
            LastSeenUtc = player.LastSeenUtc,
            IsOnline = player.IsOnline
        };
    }
}