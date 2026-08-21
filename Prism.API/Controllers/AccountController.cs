using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Application.UseCases.Auth;
namespace Prism.API.Controllers;

[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly LoginUseCase _loginUseCase;
    private readonly SyncAppProfilesUseCase _syncAppProfilesUseCase;
    private readonly UpdateAppProfileDataUseCase _updateAppProfileDataUseCase;
    private readonly GetAppProfilesUseCase _getAppProfilesUseCase;
    private readonly GetAppProfileDataUseCase _getAppProfileDataUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;
    private readonly IClientApplicationService _clientService;
    private readonly IAccountService _accountService;
    private readonly ISessionApplicationService _sessionService;

    public AccountController(IClientApplicationService clientService, LoginUseCase loginUseCase, RefreshTokenUseCase refreshTokenUseCase, IAccountService accountService, ISessionApplicationService sessionService, SyncAppProfilesUseCase syncAppProfilesUseCase, UpdateAppProfileDataUseCase updateAppProfileDataUseCase, GetAppProfilesUseCase getAppProfilesUseCase, GetAppProfileDataUseCase getAppProfileDataUseCase)
    {
        _clientService = clientService;
        _loginUseCase = loginUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _accountService = accountService;
        _sessionService = sessionService;
        _syncAppProfilesUseCase = syncAppProfilesUseCase;
        _updateAppProfileDataUseCase = updateAppProfileDataUseCase;
        _getAppProfilesUseCase = getAppProfilesUseCase;
        _getAppProfileDataUseCase = getAppProfileDataUseCase;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginClientResponse>> Login([FromBody] LoginClientRequest request)
    {
        var result = await _loginUseCase.ExecuteAsync(request);
        return FromResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginClientResponse>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _refreshTokenUseCase.ExecuteAsync(request);
        return FromResult(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var result = await _clientService.LogoutAsync();
        return FromResult(result);
    }

    [HttpGet("sessions/me")]
    [Authorize]
    public async Task<IActionResult> GetMySessions()
    {
        var clientIdClaim = User.FindFirst("client_id");

        if (clientIdClaim == null || !Guid.TryParse(clientIdClaim.Value, out var clientId))
            return Unauthorized();

        var sessions =
            await _sessionService.GetSessionsByClientIdAsync(clientId);

        return Ok(sessions);
    }

    [HttpGet("app-profiles")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AppProfileResponse>>> GetAppProfiles()
    {
        var clientIdClaim = User.FindFirst("client_id");

        if (clientIdClaim == null || !Guid.TryParse(clientIdClaim.Value, out var clientId))
            return Unauthorized();

        var result = await _getAppProfilesUseCase.ExecuteAsync(clientId);
        return Ok(result);
    }

    [HttpGet("app-profiles/{profileId:guid}")]
    [Authorize]
    public async Task<ActionResult<AppProfileResponse>> GetAppProfilesById(Guid profileId)
    {
        var clientIdClaim = User.FindFirst("client_id");

        if (clientIdClaim == null || !Guid.TryParse(clientIdClaim.Value, out var clientId))
            return Unauthorized();

        var result = await _getAppProfileDataUseCase.ExecuteAsync(clientId, profileId);
        return FromResult(result);
    }


    [HttpPut("app-profiles/sync")]
    [Authorize]
    public async Task<IActionResult> SyncAppProfiles([FromBody] SyncProfilesRequest syncProfilesRequest)
    {
        var clientIdClaim = User.FindFirst("client_id");

        if (clientIdClaim == null || !Guid.TryParse(clientIdClaim.Value, out var clientId))
            return Unauthorized();

        var result = await _syncAppProfilesUseCase.ExecuteAsync(clientId, syncProfilesRequest);

        return Ok(result);
    }

    [HttpPatch("app-profiles")]
    [Authorize]
    public async Task<IActionResult> UpdateAppProfileData([FromBody] UpdateAppProfileDataRequest req)
    {
        var clientIdClaim = User.FindFirst("client_id");

        if (clientIdClaim == null || !Guid.TryParse(clientIdClaim.Value, out var clientId))
            return Unauthorized();

        var result = await _updateAppProfileDataUseCase.ExecuteAsync(clientId, req);

        return FromResult(result);
        //return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<MeResponse>> Me()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(email))
            return Unauthorized();

        var userResult = await _accountService.FindByEmailAsync(email);

        if (!userResult.IsSuccess || userResult.Value is null)
            return NotFound();

        var user = userResult.Value;

        var clientResult = await _clientService.GetByIdAsync(user.ClientId);

        if (!clientResult.IsSuccess || clientResult.Value is null)
            return NotFound();

        var client = clientResult.Value;

        var roles =
            await _accountService.GetRolesAsync(user.Id);

        var remainingDays = Math.Max(0, (client.ExpiresAt - DateTime.UtcNow).Days);

        var response = new MeResponse
        {
            Id = user.Id,
            ClientId = user.ClientId,

            FullName = user.FullName,
            Email = user.Email,

            Roles = roles,

            License = new LicenseInfoResponse
            {
                ExpiresAt = client.ExpiresAt,
                IsExpired = client.ExpiresAt <= DateTime.UtcNow,
                RemainingDays = remainingDays
            }
        };

        return Ok(response);
    }
}