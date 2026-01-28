using FluentValidation.Results;
using Prism.Application.Common;
namespace Prism.Application.Extensions;

public static class FluentValidatorExtension
{
    public static Result ToResult(this ValidationResult validation)
    {
        if (validation.IsValid)
            return Result.Ok();

        // if you want aggregate errors
        //var message = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));

        var validError = validation.Errors
            .FirstOrDefault(ec => Enum.TryParse<ErrorCode>(ec.ErrorCode, out var parsed) && parsed != ErrorCode.None);

        if (validError is not null)
            Result.Fail(validError.ErrorMessage, Enum.Parse<ErrorCode>(validError.ErrorCode));

        // fallback
        var genericError = validation.Errors.First();
        return Result.Fail(genericError.ErrorMessage, ErrorCode.ValidationError);
    }
}