    using FluentValidation;
using Prism.Application.Common;
using Prism.Application.Dtos;
namespace Prism.Application.Validators;

public class LoginClientRequestValidator : AbstractValidator<LoginClientRequest>
{
    public LoginClientRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .WithErrorCode(nameof(ErrorCode.InvalidEmail));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .WithErrorCode(nameof(ErrorCode.PasswordTooWeak));

        RuleFor(x => x.DeviceFingerprint)
            .NotEmpty().WithMessage("Device fingerprint is required.");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required.");
    }
}