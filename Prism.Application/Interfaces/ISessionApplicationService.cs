using Prism.Application.Dtos;

namespace Prism.Application.Interfaces;
public interface ISessionApplicationService
{
    Task<IEnumerable<SessionDto>> GetSessionsByClientIdAsync(Guid clientId);
}