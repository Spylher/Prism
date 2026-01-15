using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
namespace Prism.Infrastructure.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<Result> CreateUserAsync(Guid clientId, string userName, string email, string password)
    {
        var appUser = new ApplicationUser
        {
            ClientId = clientId,
            UserName = userName,
            Email = email
        };

        var res = await _userManager.CreateAsync(appUser, password);

        return res.Succeeded
            ? Result.Ok()
            : Result.Fail(res.Errors.FirstOrDefault()?.Description ?? "Error on create user.");
    }

    public async Task<Result> ResetPasswordByClientIdAsync(Guid clientId, string newPassword)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.ClientId == clientId);

        if (user == null)
            return Result.Fail("User not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return identityResult.Succeeded ? Result.Ok() : Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }

    public async Task<Result> ResetPasswordByUserIdAsync(Guid userId, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser == null)
            return Result.Fail("User not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        var identityResult = await _userManager.ResetPasswordAsync(appUser, token, newPassword);

        return identityResult.Succeeded ? Result.Ok() : Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }

    public async Task<Result> ChangePasswordByUserIdAsync(Guid userId, string currentPassword, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser == null)
            return Result.Fail("User not found.");

        var identityResult = await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

        if (identityResult.Succeeded)
            return Result.Ok();

        if (identityResult.Errors.Any(e => e.Code == "PasswordMismatch"))
            return Result.Fail("Current password is invalid.");

        return Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }

}
