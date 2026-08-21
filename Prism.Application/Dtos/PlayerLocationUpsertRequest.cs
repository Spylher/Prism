namespace Prism.Application.Dtos;

public class PlayerLocationUpsertRequest
{
    public string PlayerName { get; set; } = default!;
    public string MapName { get; set; } = default!;
    public int X { get; set; }
    public int Y { get; set; }
    public string? GroupName { get; set; }
    public int ClassId { get; set; }
    public int Direction { get; set; }
}