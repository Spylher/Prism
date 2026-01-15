using Prism.Application.Common;
using Prism.Application.Dtos;

namespace Prism.Application.Interfaces;

public interface IAccountService
{
    Task<Result> CreateUserAsync(Guid clientId, string userName, string email, string password);
    Task<Result> ChangePasswordByUserIdAsync(Guid userId, string currentPassword, string newPassword);
    Task<Result> ResetPasswordByUserIdAsync(Guid userId, string newPassword);
    Task<Result> ResetPasswordByClientIdAsync(Guid clientId, string newPassword);
}
