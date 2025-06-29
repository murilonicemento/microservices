using FluentValidation;
using UserService.Core.DTO;

namespace UserService.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(temp => temp.Email)
            .EmailAddress()
            .WithMessage("Invalid email address format.")
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(temp => temp.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password minimum length is 8.");

        RuleFor(temp => temp.PersonName)
            .NotEmpty()
            .WithMessage("Person Name is required.")
            .MinimumLength(1)
            .MaximumLength(50)
            .WithMessage("Password maximum length is 50.");

        RuleFor(temp => temp.Gender)
            .IsInEnum()
            .WithMessage("Gender must be Male, Female or Other.");
    }
}