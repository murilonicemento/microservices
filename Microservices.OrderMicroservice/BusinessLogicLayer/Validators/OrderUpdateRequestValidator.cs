using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(temp => temp.OrderId)
            .NotEmpty()
            .WithMessage("Order ID can't be blank");

        RuleFor(temp => temp.UserId)
            .NotEmpty()
            .WithMessage("User ID can't be blank");

        RuleFor(temp => temp.OrderDate)
            .NotEmpty()
            .WithMessage("Order date can't be blank");
    }
}