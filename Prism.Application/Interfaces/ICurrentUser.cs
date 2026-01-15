using Prism.Application.Common;
using Prism.Application.Dtos;

namespace Prism.Application.Interfaces;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Task<Result<UserReadModel>> GetUserAsync();
    Task<Guid?> GetClientIdAsync();
    Task<Guid?> GetUserIdAsync();
}
