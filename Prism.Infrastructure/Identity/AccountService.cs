using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
namespace Prism.Infrastructure.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IClientRepository _clientRepository;


    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IClientRepository clientRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _clientRepository = clientRepository;
    }

    public async Task<Result> SignInAsync(string email, string password, bool rememberMe)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Fail("Invalid credentials.", ErrorCode.Unauthorized);

        var signInResult = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: true);

        if (signInResult.Succeeded)
            return Result.Ok();

        if (signInResult.IsLockedOut)
            return Result.Fail("User is locked out.", ErrorCode.Forbidden);

        return Result.Fail("Invalid credentials.", ErrorCode.Unauthorized);
    }

    public async Task<Result> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return Result.Ok();
    }

    public async Task<Result> CreateUserAsync(Guid clientId, string fullName, string email, string password)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
            return Result.Fail("Email already registered.", ErrorCode.EmailAlreadyInUse);

        const string clientRole = "Client";

        var appUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            UserName = email,
            FullName = fullName,
            NormalizedUserName = email.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            //EmailConfirmed = false // if you want for email confirmation
        };

        var res = await _userManager.CreateAsync(appUser, password);
        await _userManager.AddToRoleAsync(appUser, clientRole);

        return res.Succeeded
            ? Result.Ok()
            : Result.Fail(res.Errors.FirstOrDefault()?.Description ?? "Error on create user.");
    }

    public async Task<Result> ResetPasswordByClientIdAsync(Guid clientId, string newPassword)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.ClientId == clientId);

        if (user == null)
            return Result.Fail("User not found.", ErrorCode.NotFound);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return identityResult.Succeeded ? Result.Ok() : Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }

    public async Task<Result> ResetPasswordByUserIdAsync(Guid userId, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser == null)
            return Result.Fail("User not found.", ErrorCode.NotFound);

        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        var identityResult = await _userManager.ResetPasswordAsync(appUser, token, newPassword);

        return identityResult.Succeeded ? Result.Ok() : Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }

    public async Task<Result> ChangePasswordByUserIdAsync(Guid userId, string currentPassword, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser == null)
            return Result.Fail("User not found.", ErrorCode.NotFound);

        var identityResult = await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

        if (identityResult.Succeeded)
            return Result.Ok();

        if (identityResult.Errors.Any(e => e.Code == "PasswordMismatch"))
            return Result.Fail("Current password is invalid.");

        return Result.Fail(identityResult.Errors.FirstOrDefault()?.Description ?? "Error on change password.");
    }


    public async Task<Result<ApplicationUserDto>> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<ApplicationUserDto>.Fail("User not found.", ErrorCode.NotFound);

        return Result<ApplicationUserDto>.Ok(ToDto(user));
    }

    public async Task<Result<ApplicationUserDto>> FindByClientIdAsync(Guid clientId)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.ClientId == clientId);
        if (user is null)
            return Result<ApplicationUserDto>.Fail("User not found.", ErrorCode.NotFound);

        return Result<ApplicationUserDto>.Ok(ToDto(user));
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUserDto dto, string password)
    {
        var user = await _userManager.FindByIdAsync(dto.Id.ToString());
        if (user is null)
            return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserResponse>();

        foreach (var user in users)
        {
            var client = await _clientRepository.GetByIdAsync(user.ClientId);
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserResponse(user.Id, user.Email!, user.FullName, client.Id, client.ExpiresAt, roles));
        }

        return result;
    }

    private static ApplicationUserDto ToDto(ApplicationUser user) => new(user.Id, user.ClientId, user.FullName, user.Email!);
}
