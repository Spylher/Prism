using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using System.Security.Claims;
namespace Prism.Infrastructure.Identity;

public class IdentityCurrentUser : ICurrentUser
{
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private Task<Result<UserReadModel>>? _cachedUserReadModelTask;

    public IdentityCurrentUser(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public Task<Result<UserReadModel>> GetUserAsync()
    {
        if (_cachedUserReadModelTask != null)
            return _cachedUserReadModelTask;

        _cachedUserReadModelTask = LoadUserAsync();
        return _cachedUserReadModelTask;
    }

    private async Task<Result<UserReadModel>> LoadUserAsync()
    {
        if (!IsAuthenticated || Principal == null)
            return Result<UserReadModel>.Fail("Unauthenticated user.");

        var appUser = await _userManager.GetUserAsync(Principal);
        if (appUser == null)
            return Result<UserReadModel>.Fail("User not found.");

        var userReadModel = new UserReadModel(appUser.Id, appUser.UserName ?? string.Empty, appUser.Email ?? string.Empty, appUser.ClientId);
        return Result<UserReadModel>.Ok(userReadModel);
    }

    public async Task<Guid?> GetUserIdAsync()
    {
        var userResult = await GetUserAsync();
        if (!userResult.IsSuccess)
            return null;

        return userResult.Value?.Id;
    }

    public async Task<Guid?> GetClientIdAsync()
    {
        var userResult = await GetUserAsync();
        if (!userResult.IsSuccess)
            return null;

        return userResult.Value?.ClientId;
    }
}
