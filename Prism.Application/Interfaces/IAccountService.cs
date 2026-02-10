using Prism.Application.Common;
namespace Prism.Application.Interfaces;

public interface IAccountService
{
    Task<Result> SignOutAsync();
    Task<Result> SignInAsync(string email, string password, bool rememberMe);
    Task<Result> CreateUserAsync(Guid clientId, string fullName, string email, string password);
    Task<Result> ChangePasswordByUserIdAsync(Guid userId, string currentPassword, string newPassword);
    Task<Result> ResetPasswordByUserIdAsync(Guid userId, string newPassword);
    Task<Result> ResetPasswordByClientIdAsync(Guid clientId, string newPassword);
}
