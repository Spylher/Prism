using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
namespace Prism.API.Controllers;

[Authorize]
[Route("api/profile")]
public class ProfileController : BaseApiController
{
    private readonly IClientApplicationService _clientService;

    public ProfileController(IClientApplicationService service)
    {
        _clientService = service;
    }

    [HttpGet]
    public async Task<ActionResult<ClientProfileDto>> GetProfile()
    {
        var result = await _clientService.GetProfileAsync();
        return FromResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateClientRequest request)
    {
        var result = await _clientService.UpdateProfileAsync(request);
        return FromResult(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
    {
        var result = await _clientService.ChangePasswordAsync(dto.CurrentPassword, dto.NewPassword);
        return FromResult(result);
    }
}

