using Microsoft.AspNetCore.Mvc;
using Prism.Application.Common;
namespace Prism.API.Controllers;

[ApiController]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{
    protected IActionResult FromResult(Result result)
    {
        if (result.IsSuccess)
            return Ok();

        return result.ErrorCode switch
        {
            ErrorCode.Unauthorized => Unauthorized(),
            ErrorCode.Forbidden => Forbid(),
            ErrorCode.NotFound or ErrorCode.ClientNotFound => NotFound(new { error = result.Error }),
            ErrorCode.ValidationError => BadRequest(new { error = result.Error }),
            ErrorCode.Conflict or ErrorCode.EmailAlreadyInUse => Conflict(new { error = result.Error }),
            ErrorCode.InvalidEmail or ErrorCode.PasswordTooWeak => BadRequest(new { error = result.Error }),

            _ => StatusCode(500, new { error = result.Error })
        };

        //StatusCodes.Status404NotFound;
    }

    protected ActionResult<T> FromResult<T>(Result<T> result)
    {
        if (result is { IsSuccess: true })
            return Ok(result.Value);

        //if (result.IsSuccess)
        //    return NotFound();

        return result.ErrorCode switch
        {
            ErrorCode.Unauthorized => Unauthorized(),
            ErrorCode.Forbidden => Forbid(),
            ErrorCode.NotFound or ErrorCode.ClientNotFound => NotFound(new { error = result.Error }),
            ErrorCode.ValidationError => BadRequest(new { error = result.Error }),
            ErrorCode.EmailAlreadyInUse => Conflict(new { error = result.Error }),
            ErrorCode.InvalidEmail or ErrorCode.PasswordTooWeak => BadRequest(new { error = result.Error }),

            _ => StatusCode(500, new { error = result.Error })
        };

    }
}
