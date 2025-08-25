using ChallengeCore.Application.Services.Purchases;
using ChallengeCore.Domain.Models;

namespace ChallengeUI.EndPoints;

public static class PurchaseEndPoints
{
    public static void MapPurchases(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/purchases");

        _ = group.MapGet("/", (IPurchaseService purchaseService) =>
        {
            var result = purchaseService.GetAll();
            return Results.Ok(result.Value);
        });

        _ = group.MapGet("/{id:int}", (IPurchaseService purchaseService, int id) =>
        {
            var result = purchaseService.GetById(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });

        _ = group.MapPost("/", (IPurchaseService purchaseService, Purchase purchase) =>
        {
            var result = purchaseService.Add(purchase);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        });

        _ = group.MapPut("/", (IPurchaseService purchaseService, Purchase purchase) =>
        {
            var result = purchaseService.Update(purchase);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        });

        _ = group.MapDelete("/{id:int}", (IPurchaseService purchaseService, int id) =>
        {
            var result = purchaseService.Delete(id);
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        });
    }
}