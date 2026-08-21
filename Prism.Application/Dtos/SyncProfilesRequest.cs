namespace Prism.Application.Dtos;

public record SyncProfilesRequest (IEnumerable<string> Profiles);