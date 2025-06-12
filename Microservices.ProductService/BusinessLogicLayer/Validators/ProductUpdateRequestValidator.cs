using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(temp => temp.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(temp => temp.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.");

        RuleFor(temp => temp.Category)
            .IsInEnum()
            .WithMessage("Category not valid.");

        RuleFor(temp => temp.UnitPrice)
            .InclusiveBetween(0, double.MaxValue)
            .WithErrorCode($"Unit price should between 0 to {double.MaxValue}");

        RuleFor(temp => temp.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue)
            .WithMessage($"Quantity in stock should between 0 to {int.MaxValue}");
    }
}