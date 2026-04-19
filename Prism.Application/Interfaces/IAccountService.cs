using Prism.Application.Common;
using Prism.Application.Dtos;
namespace Prism.Application.Interfaces;

public interface IAccountService
{
    Task<Result> SignOutAsync();
    Task<Result> SignInAsync(string email, string password, bool rememberMe);
    Task<Result> CreateUserAsync(Guid clientId, string fullName, string email, string password);
    Task<Result> ChangePasswordByUserIdAsync(Guid userId, string currentPassword, string newPassword);
    Task<Result> ResetPasswordByUserIdAsync(Guid userId, string newPassword);
    Task<Result> ResetPasswordByClientIdAsync(Guid clientId, string newPassword);
    Task<Result<ApplicationUserDto>> FindByEmailAsync(string email);
    Task<Result<ApplicationUserDto>> FindByClientIdAsync(Guid clientId);
    Task<bool> CheckPasswordAsync(ApplicationUserDto user, string password);
    Task<IList<string>> GetRolesAsync(Guid userId);
    Task<List<UserResponse>> GetAllUsersAsync();
}
