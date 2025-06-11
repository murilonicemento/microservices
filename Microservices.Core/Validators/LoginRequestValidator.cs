using FluentValidation;
using Microservices.Core.DTO;

namespace Microservices.Core.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(temp => temp.Email)
            .EmailAddress()
            .WithMessage("Invalid email address format.")
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(temp => temp.Password)
            .NotEmpty()
            .WithMessage("Email is required.");
    }
}