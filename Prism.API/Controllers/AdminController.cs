using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Application.UseCases.Auth;

namespace Prism.API.Controllers;

[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    private readonly IClientApplicationService _clientService;
    private readonly IAccountService _accountService;
    private readonly ISessionApplicationService _sessionService;
    private readonly AddDaysToClientUseCase _addDaysToClientUseCase;
    private readonly SyncDiscordUseCase _syncDiscordUseCase;

    public AdminController(IClientApplicationService clientService, IAccountService accountService, ISessionApplicationService sessionService,
        AddDaysToClientUseCase addDaysToClientUseCase, SyncDiscordUseCase syncDiscordUseCase)
    {
        _clientService = clientService;
        _accountService = accountService;
        _sessionService = sessionService;
        _addDaysToClientUseCase = addDaysToClientUseCase;
        _syncDiscordUseCase = syncDiscordUseCase;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterClientRequest request)
    {
        var result = await _clientService.RegisterAsync(request);
        return FromResult(result);
    }

    [HttpPost("{clientId:guid}/add-days")]
    public async Task<IActionResult> AddDays(Guid clientId, [FromBody] AddDaysRequest request)
    {
        var result = await _addDaysToClientUseCase.ExecuteAsync(clientId, request.Days);
        return FromResult(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _accountService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("sync-discord")]
    public async Task<IActionResult> SyncDiscord([FromQuery] string? email, [FromQuery] Guid? clientId, [FromBody] DiscordProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(email) && clientId == null)
            return BadRequest("Email or clientId is required.");

        Guid resolvedClientId;

        if (!string.IsNullOrWhiteSpace(email))
        {
            var userResult =
                await _accountService.FindByEmailAsync(email);

            if (!userResult.IsSuccess || userResult.Value is null)
                return NotFound("User not found.");

            resolvedClientId = userResult.Value.ClientId;
        }
        else
        {
            resolvedClientId = clientId!.Value;
        }

        var result = await _syncDiscordUseCase.ExecuteAsync(resolvedClientId, request);
        return FromResult(result);
    }

    [HttpPatch("clients/license")]
    public async Task<IActionResult> AddDays([FromQuery] string? email, [FromQuery] Guid? clientId, [FromQuery] int days)
    {
        if (string.IsNullOrWhiteSpace(email) && clientId == null)
            return BadRequest("Email or clientId is required.");

        Guid resolvedClientId;

        if (!string.IsNullOrWhiteSpace(email))
        {
            var userResult =
                await _accountService.FindByEmailAsync(email);

            if (!userResult.IsSuccess || userResult.Value is null)
                return NotFound("User not found.");

            resolvedClientId = userResult.Value.ClientId;
        }
        else
        {
            resolvedClientId = clientId!.Value;
        }

        var result = await _addDaysToClientUseCase.ExecuteAsync(resolvedClientId, days);
        return FromResult(result);
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions([FromQuery] string? email, [FromQuery] Guid? clientId)
    {
        if (string.IsNullOrWhiteSpace(email) && clientId == null)
            return BadRequest("Email or clientId is required.");

        Guid resolvedClientId;

        if (!string.IsNullOrWhiteSpace(email))
        {
            var userResult = await _accountService.FindByEmailAsync(email);

            if (!userResult.IsSuccess || userResult.Value is null)
                return NotFound("User not found.");

            resolvedClientId = userResult.Value.ClientId;
        }
        else
        {
            resolvedClientId = clientId!.Value;
        }

        var sessions = await _sessionService.GetSessionsByClientIdAsync(resolvedClientId);
        return Ok(sessions);
    }
}