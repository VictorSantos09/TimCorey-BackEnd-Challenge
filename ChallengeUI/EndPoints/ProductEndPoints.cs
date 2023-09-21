using ChallengeCore.DTOs;
using ChallengeCore.Services;

namespace ChallengeUI.EndPoints;

public static class ProductEndPoints
{
    private static readonly ProductService _service = new();

    public static void MapProductsEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products");
        group.MapPost("/viewUsersProducts", async (string email) =>
        {
            var result = await _service.GetUserProducts(email);
            return Results.Ok(result);
        });

        group.MapPost("/buy", async (BuyProductDTO dto) =>
        {
            var result = await _service.Buy(dto);
            return result.Success ? BaseDTO.Valid("produto comprado") : BaseDTO.Invalid("produto não comprado", result.Message);
        });

        group.MapGet("/viewProducts", async () =>
        {
            var result = await _service.ViewAll();
            return result;
        });
    }
}
