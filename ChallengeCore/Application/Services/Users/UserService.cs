using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Users;
internal class UserService(IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    public Result Add(User entity)
    {
        logger.LogInformation("Adicionando usuário: {@User}", entity);
        if (entity == null)
        {
            logger.LogWarning("Usuário nulo ao adicionar.");
            return Result.Fail("Usuário nulo.");
        }

        userRepository.Add(entity);
        logger.LogInformation("Usuário adicionado com sucesso: {@User}", entity);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.LogInformation("Removendo usuário por Id: {Id}", id);
        var user = userRepository.GetById(id);
        if (user == null)
        {
            logger.LogWarning("Usuário não encontrado para remoção: {Id}", id);
            return Result.Fail("Usuário não encontrado.");
        }

        userRepository.Delete(user);
        logger.LogInformation("Usuário removido com sucesso: {@User}", user);
        return Result.Ok();
    }

    public Result<IEnumerable<User>> GetAll()
    {
        logger.LogInformation("Buscando todos os usuários.");
        var users = userRepository.GetAll();
        logger.LogInformation("Total de usuários encontrados: {Count}", users.Count());
        return Result.Ok(users);
    }

    public Result<User> GetById(int id)
    {
        logger.LogInformation("Buscando usuário por Id: {Id}", id);
        var user = userRepository.GetById(id);
        if (user == null)
        {
            logger.LogWarning("Usuário não encontrado para o Id: {Id}", id);
            return Result.Fail<User>("Usuário não encontrado.");
        }
        logger.LogInformation("Usuário encontrado: {@User}", user);
        return Result.Ok(user);
    }

    public Result Update(User entity)
    {
        logger.LogInformation("Atualizando usuário: {@User}", entity);
        if (entity == null)
        {
            logger.LogWarning("Usuário nulo ao atualizar.");
            return Result.Fail("Usuário nulo.");
        }

        var existing = userRepository.GetById(entity.Id);
        if (existing == null)
        {
            logger.LogWarning("Usuário não encontrado para atualização: {Id}", entity.Id);
            return Result.Fail("Usuário não encontrado.");
        }

        userRepository.Update(entity);
        logger.LogInformation("Usuário atualizado com sucesso: {@User}", entity);
        return Result.Ok();
    }
}