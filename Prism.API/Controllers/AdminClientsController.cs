using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Prism.Application.Interfaces;
namespace Prism.API.Controllers;

[Route("api/admin/clients")]
[Authorize(Roles = "Admin")]
public class AdminClientsController : BaseApiController
{
    private readonly IClientApplicationService _clientService;

    public AdminClientsController(IClientApplicationService clientService)
    {
        _clientService = clientService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{userId:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid userId, [FromBody] ResetPasswordRequest dto)
    {
        var result = await _clientService.ResetPasswordAsync(userId, dto.NewPassword);
        return FromResult(result);
    }
}
