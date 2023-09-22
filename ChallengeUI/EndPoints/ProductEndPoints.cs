using ChallengeCore.DTOs;
using ChallengeCore.Services;

namespace ChallengeUI.EndPoints;

public static class ProductEndPoints
{
    private static readonly ProductService _productService = new();

    public static void MapProductsEndPoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("api/products");
        _ = group.MapPost("/viewUsersProducts", async (string email) =>
        {
            BaseDTO result = await _productService.GetUserProducts(email);
            return Results.Ok(result);
        });

        _ = group.MapPost("/buy", async (BuyProductDTO dto) =>
        {
            BaseDTO result = await _productService.Buy(dto);
            return result.Success ? BaseDTO.Valid("produto comprado") : BaseDTO.Invalid("produto não comprado", result.Message);
        });

        _ = group.MapGet("/viewProducts", async () =>
        {
            BaseDTO result = await _productService.ViewAll();
            return result;
        });
    }
}
