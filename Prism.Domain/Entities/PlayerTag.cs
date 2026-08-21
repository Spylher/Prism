namespace Prism.Domain.Entities;

public class PlayerTag
{
    public Guid Id { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public string PlayerName { get; private set; } = default!;
    public string PlayerNameNormalized { get; private set; } = default!;
    public string MapName { get; private set; } = default!;
    public int X { get; private set; }
    public int Y { get; private set; }
    public string? GroupName { get; private set; }
    public int ClassId { get; private set; }
    public int Direction { get; private set; }
    public DateTime FirstSeenUtc { get; private set; }
    public DateTime LastSeenUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public bool IsOnline => DateTime.UtcNow - LastSeenUtc <= TimeSpan.FromSeconds(15);

    protected PlayerTag() { }

    public PlayerTag(Guid ownerUserId, string playerName, string mapName, int x, int y, string? groupName, int classId, int direction, DateTime nowUtc)
    {
        Id = Guid.NewGuid();

        OwnerUserId = ownerUserId;
        PlayerName = playerName;
        PlayerNameNormalized = playerName.ToUpperInvariant();
        MapName = mapName;

        X = x;
        Y = y;

        GroupName = groupName;
        ClassId = classId;
        Direction = direction;
        FirstSeenUtc = nowUtc;
        LastSeenUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void Update(string mapName, int x, int y, string? groupName, int classId, int direction, DateTime nowUtc)
    {
        MapName = mapName;

        X = x;
        Y = y;

        GroupName = groupName;
        ClassId = classId;
        Direction = direction;
        LastSeenUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void Touch(DateTime nowUtc)
    {
        LastSeenUtc = nowUtc;
    }
}