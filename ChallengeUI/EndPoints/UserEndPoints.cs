using ChallengeCore.Application.Services.Users;
using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Logging;

namespace ChallengeUI.EndPoints;

public static class UserEndPoints
{
    public static void MapUser(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users");

        _ = group.MapPost("/", (IUserService userService, ILogger<UserEndPointsLogger> logger, User user) =>
        {
            var result = userService.Add(user);
            logger.Information("Resultado do registro de usuário: {Success}", result.IsSuccess);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        });

        _ = group.MapGet("/", (IUserService userService, ILogger<UserEndPointsLogger> logger) =>
        {
            var result = userService.GetAll();
            logger.Information("Total de usuários retornados: {Count}", result.Value?.Count() ?? 0);
            return Results.Ok(result.Value);
        });

        _ = group.MapGet("/{id:int}", (IUserService userService, ILogger<UserEndPointsLogger> logger, int id) =>
        {
            var result = userService.GetById(id);
            logger.Information("Usuário encontrado: {Success}", result.IsSuccess);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });

        _ = group.MapPut("/", (IUserService userService, ILogger<UserEndPointsLogger> logger, User entity) =>
        {
            var result = userService.Update(entity);
            logger.Information("Resultado da atualização de usuário: {Success}", result.IsSuccess);
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result.Errors);
        });

        _ = group.MapDelete("/{id:int}", (IUserService userService, ILogger<UserEndPointsLogger> logger, int id) =>
        {
            var result = userService.Delete(id);
            logger.Information("Resultado da remoção de usuário: {Success}", result.IsSuccess);
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result.Errors);
        });
    }
}
