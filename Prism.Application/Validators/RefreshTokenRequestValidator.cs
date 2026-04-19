using FluentValidation;
using Prism.Application.Dtos;
namespace Prism.Application.Validators;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
        RuleFor(x => x.DeviceFingerprint).NotEmpty();
    }
}