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
}