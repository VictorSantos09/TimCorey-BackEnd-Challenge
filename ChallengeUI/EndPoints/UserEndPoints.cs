using ChallengeCore.DTOs;
using ChallengeCore.Services;

namespace ChallengeUI.EndPoints
{
    public static class UserEndPoints
    {
        private static readonly UserService _service = new();

        public static void MapUserEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/users");
            group.MapPost("/register", async (UserDTO dto) =>
            {
                var result = await _service.Register(dto);
                return result.Success ? Results.Ok("Conta criada com sucesso") : Results.BadRequest("Conta não criada");
            }).WithName("register");

            group.MapPost("/view", async (string email, string password) =>
            {
                var result = await _service.ViewInfo(email, password);
                return result.Success ? Results.Ok("Usuário encontrado") : Results.BadRequest("Usuário não existente");
            }).WithName("view");
        }
    }
}