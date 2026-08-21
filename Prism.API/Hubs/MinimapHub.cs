using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using System.Collections.Concurrent;

namespace Prism.API.Hubs;

[Authorize]
public class MinimapHub : Hub
{
    private readonly IPlayerTagRealtimeService _service;
    private static readonly ConcurrentDictionary<string, PlayerConnectionInfo>
        _connections = new();

    public MinimapHub(IPlayerTagRealtimeService service)
    {
        _service = service;
    }

    public async Task JoinMap(string mapName, string? playerName = null)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetMapGroup(mapName));

        if (!string.IsNullOrWhiteSpace(playerName))
            _connections[Context.ConnectionId] = new PlayerConnectionInfo { PlayerName = playerName, MapName = mapName };

        var snapshot = await _service.GetMapSnapshotAsync(mapName);

        await Clients.Caller.SendAsync("map-snapshot", snapshot);
    }

    public async Task LeaveMap(string mapName, string playerName)
    {
        var groupName = GetMapGroup(mapName);

        // sai do grupo
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        // avisa os outros players do mapa
        await Clients.Group(groupName).SendAsync("player-removed", playerName);
    }

    public async Task UpsertLocation(PlayerLocationUpsertRequest request)
    {
        var userId = Guid.Parse(Context.User!.FindFirst("client_id")!.Value);

        var dto = await _service.UpsertAsync(userId, request, DateTime.UtcNow);

        await Clients.Group(GetMapGroup(request.MapName))
            .SendAsync("player-updated", dto);
    }

    public async Task Ping(string playerName)
    {
        await _service.TouchAsync(playerName, DateTime.UtcNow);
    }

    private static string GetMapGroup(string map)
    {
        return $"map:{map.ToUpperInvariant()}";
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connections.TryRemove(Context.ConnectionId, out var info))
        {
            await Clients.Group(GetMapGroup(info.MapName))
                .SendAsync("player-removed", info.PlayerName);
        }

        await base.OnDisconnectedAsync(exception);
    }
}