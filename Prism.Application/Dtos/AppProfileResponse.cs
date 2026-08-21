namespace Prism.Application.Dtos;

public record AppProfileResponse(Guid ProfileId, string Name, string? Data = null);
