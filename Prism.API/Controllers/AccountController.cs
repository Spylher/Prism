using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Application.UseCases.Auth;
namespace Prism.API.Controllers;

[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly IClientApplicationService _clientService;
    private readonly LoginUseCase _loginUseCase;
    private readonly AddDaysToClientUseCase _addDaysToClientUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;
    private readonly IValidator<LoginClientRequest> _loginValidator;
    private readonly IAccountService _accountService;
    private readonly ISessionApplicationService _sessionService;

    public AccountController(IClientApplicationService clientService, LoginUseCase loginUseCase, RefreshTokenUseCase refreshTokenUseCase, IValidator<LoginClientRequest> loginValidator, IAccountService accountService, AddDaysToClientUseCase addDaysToClientUseCase, ISessionApplicationService sessionService)
    {
        _clientService = clientService;
        _loginUseCase = loginUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _loginValidator = loginValidator;
        _accountService = accountService;
        _addDaysToClientUseCase = addDaysToClientUseCase;
        _sessionService = sessionService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterClientRequest request)
    {
        var result = await _clientService.RegisterAsync(request);
        return FromResult(result);
    }


    // cookie-based authentication endpoints (commented out for now, as we're using JWTs instead)
    //[HttpPost("login")]
    //public async Task<IActionResult> Login([FromBody] LoginClientRequest clientRequest)
    //{
    //    var result = await _clientService.LoginAsync(clientRequest);
    //    return FromResult(result);
    //}
    //[HttpPost("logout")]
    //[Authorize]
    //public async Task<IActionResult> Logout()
    //{
    //    var result = await _clientService.LogoutAsync();
    //    return FromResult(result);
    //}

    [HttpPost("{clientId:guid}/add-days")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDays(Guid clientId, [FromBody] AddDaysRequest request)
    {
        var result = await _addDaysToClientUseCase.ExecuteAsync(clientId, request.Days);
        return FromResult(result);
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

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _accountService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("sessions")]
    [Authorize]
    public async Task<IActionResult> GetMySessions()
    {
        var clientId = Guid.Parse(User.FindFirst("client_id")!.Value);

        var sessions = await _sessionService.GetSessionsByClientIdAsync(clientId);

        return Ok(sessions);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Admin")]
    public IActionResult Me()
    {
        return Ok(new
        {
            User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}