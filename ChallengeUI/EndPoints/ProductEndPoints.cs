using ChallengeCore.Application.Services.Products;
using ChallengeCore.Domain.Models;

namespace ChallengeUI.EndPoints;

public static class ProductEndPoints
{
    public static void MapProducts(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("api/products");

        _ = group.MapGet("/", (IProductService productService) =>
        {
            var result = productService.GetAll();
            return Results.Ok(result.Value);
        });

        _ = group.MapGet("/{id:int}", (IProductService productService, int id) =>
        {
            var result = productService.GetById(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });

        _ = group.MapPost("/", (IProductService productService, Product entity) =>
        {
            var result = productService.Add(entity);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        });

        _ = group.MapPut("/", (IProductService productService, Product entity) =>
        {
            var result = productService.Update(entity);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        });

        _ = group.MapDelete("/{id:int}", (IProductService productService, int id) =>
        {
            var result = productService.Delete(id);
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        });
    }
}
