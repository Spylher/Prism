using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
namespace Prism.API.Controllers;

[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly IClientApplicationService _clientService;

    public AccountController(IClientApplicationService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterClientRequest request)
    {
        var result = await _clientService.RegisterAsync(request);
        return FromResult(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginClientRequest clientRequest)
    {
        var result = await _clientService.LoginAsync(clientRequest);
        return FromResult(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var result = await _clientService.LogoutAsync();
        return FromResult(result);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new
        {
            User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}