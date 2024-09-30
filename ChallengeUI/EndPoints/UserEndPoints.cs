using ChallengeCore.DTOs;
using ChallengeCore.Models;
using ChallengeCore.Services;

namespace ChallengeUI.EndPoints;

public static class UserEndPoints
{
    private static readonly UserService _userService;

    public static void MapUserEndPoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("api/users");
        _ = group.MapPost("/register", async (UserDTO dto) =>
        {
            BaseDTO result = await _userService.Register(dto);
            return result.Success ? Results.Ok(new { result.Message }) : Results.BadRequest(new { result.Message });
        });

        _ = group.MapPost("/view", async (string email, string nickname) =>
        {
            BaseDTO result = await _userService.ViewInfo(email, nickname);
            return result.Success ? Results.Ok(result.Data) : Results.BadRequest(new { result.Message });
        });
    }
}