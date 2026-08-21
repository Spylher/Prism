namespace Prism.Application.Dtos;

public class PlayerTagDto
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string PlayerName { get; set; } = default!;
    public string MapName { get; set; } = default!;
    public int X { get; set; }
    public int Y { get; set; }
    public string? GroupName { get; set; }
    public int ClassId { get; set; }
    public int Direction { get; set; }
    public DateTime LastSeenUtc { get; set; }
    public bool IsOnline { get; set; }
}