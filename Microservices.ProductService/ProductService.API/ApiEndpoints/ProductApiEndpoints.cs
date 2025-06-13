using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServicesContracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.ApiEndpoints;

public static class ProductApiEndpoints
{
    public static IEndpointRouteBuilder MapProductApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async ([FromServices] IProductService productService) =>
        {
            var products = await productService.GetProducts();

            return Results.Ok(products);
        });

        app.MapGet("/api/products/search/product-id/{productId:guid}",
            async ([FromServices] IProductService productService, [FromRoute] Guid productId) =>
            {
                var product = await productService.GetProductByCondition(temp => temp.ProductId == productId);

                return Results.Ok(product);
            });

        app.MapGet("/api/products/search/{searchString}",
            async ([FromServices] IProductService productService, [FromRoute] string searchString) =>
            {
                var products =
                    await productService.GetProductsByCondition(temp => temp.ProductName.Contains(searchString));

                return Results.Ok(products);
            });

        app.MapPost("/api/products",
            async ([FromServices] IProductService productService, [FromServices] IValidator<ProductAddRequest> productAddRequestValidator,
                [FromBody] ProductAddRequest? productAddRequest) =>
            {
                if (productAddRequest is null)
                    return Results.BadRequest("Invalid registration data.");

                var validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);
                var errorMessages = GetErrorMessages(validationResult);


                if (errorMessages.Count > 0)
                    return Results.ValidationProblem(errorMessages);

                var product = await productService.AddProduct(productAddRequest);

                return product == null
                    ? Results.Problem("Error in adding product")
                    : Results.Created($"/api/products/search/product-id/{product.ProductId}", product);
            });

        app.MapPut("/api/products",
            async ([FromServices] IProductService productService, [FromServices] IValidator<ProductUpdateRequest> productUpdateRequestValidator,
                [FromBody] ProductUpdateRequest? productUpdateRequest) =>
            {
                if (productUpdateRequest is null)
                    return Results.BadRequest("Invalid update data.");

                var validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
                var errorMessages = GetErrorMessages(validationResult);

                if (errorMessages.Values.Count > 0)
                    return Results.ValidationProblem(errorMessages);

                var product = await productService.UpdateProduct(productUpdateRequest);

                return product == null
                    ? Results.Problem("Error in updating product")
                    : Results.Ok(product);
            });

        app.MapDelete("/api/products/{productId:guid}",
            async ([FromServices] IProductService productService, [FromRoute] Guid productId) =>
            {
                var isDeleted = await productService.DeleteProduct(productId);

                return !isDeleted ? Results.Problem("Error in deleting product.") : Results.Ok(true);
            });

        return app;
    }

    private static Dictionary<string, string[]> GetErrorMessages(ValidationResult validationResult)
    {
        if (validationResult.IsValid) return new Dictionary<string, string[]>();

        var errorMessages = validationResult.Errors
            .GroupBy(temp => temp.PropertyName)
            .ToDictionary(
                grp => grp.Key,
                grp => grp.Select(err => err.ErrorMessage).ToArray()
            );

        return errorMessages;
    }
}