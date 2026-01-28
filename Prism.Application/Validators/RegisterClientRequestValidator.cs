using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Prism.Application.Common;
using Prism.Application.Dtos;
namespace Prism.Application.Validators;

public class RegisterClientRequestValidator : AbstractValidator<RegisterClientRequest>
{
    public RegisterClientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name is too long. A maximum of 100 characters is supported.");

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithMessage("Last name is required.")
            .MaximumLength(100)
                .WithMessage("Last name is too long. A maximum of 100 characters is supported.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress()
                .WithMessage("Email format is invalid.")
                .WithErrorCode(nameof(ErrorCode.InvalidEmail));

        RuleFor(x => x.Password)
            .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .WithErrorCode(nameof(ErrorCode.PasswordTooWeak))
            .Matches("[A-Za-z]")
                .WithMessage("Password must contain at least one letter.")
                .WithErrorCode(nameof(ErrorCode.PasswordTooWeak))
            .Matches("[0-9]")
                .WithMessage("Password must contain at least one number.")
                .WithErrorCode(nameof(ErrorCode.PasswordTooWeak))
            .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one non-alphanumeric character.")
                .WithErrorCode(nameof(ErrorCode.PasswordTooWeak));
    }
}
