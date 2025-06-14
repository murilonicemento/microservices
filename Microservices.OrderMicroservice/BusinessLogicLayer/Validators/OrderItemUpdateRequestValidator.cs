﻿using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class OrderItemUpdateRequestValidator : AbstractValidator<OrderItemUpdateRequest>
{
    public OrderItemUpdateRequestValidator()
    {
        RuleFor(temp => temp.ProductId)
            .NotEmpty()
            .WithMessage("Product ID can't be blank");

        RuleFor(temp => temp.UnitPrice)
            .NotEmpty()
            .WithMessage("Unit price can't be blank")
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");

        RuleFor(temp => temp.Quantity)
            .NotEmpty()
            .WithMessage("Quantity can't be blank")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");
    }
}