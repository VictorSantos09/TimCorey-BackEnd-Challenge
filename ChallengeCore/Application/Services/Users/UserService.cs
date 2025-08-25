using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Users;
internal class UserService(IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    public Result Add(User entity)
    {
        logger.Information("Adicionando usuário: {@User}", entity);
        if (entity == null)
        {
            logger.Warning("Usuário nulo ao adicionar.");
            return Result.Fail("Usuário nulo.");
        }

        userRepository.Add(entity);
        logger.Information("Usuário adicionado com sucesso: {@User}", entity);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.Information("Removendo usuário por Id: {Id}", id);
        User? user = userRepository.GetById(id);
        if (user == null)
        {
            logger.Warning("Usuário não encontrado para remoção: {Id}", id);
            return Result.Fail("Usuário não encontrado.");
        }

        userRepository.Delete(user);
        logger.Information("Usuário removido com sucesso: {@User}", user);
        return Result.Ok();
    }

    public Result<IEnumerable<User>> GetAll()
    {
        logger.Information("Buscando todos os usuários.");
        IEnumerable<User> users = userRepository.GetAll();
        logger.Information("Total de usuários encontrados: {Count}", users.Count());
        return Result.Ok(users);
    }

    public Result<User> GetById(int id)
    {
        logger.Information("Buscando usuário por Id: {Id}", id);
        User? user = userRepository.GetById(id);
        if (user == null)
        {
            logger.Warning("Usuário não encontrado para o Id: {Id}", id);
            return Result.Fail<User>("Usuário não encontrado.");
        }
        logger.Information("Usuário encontrado: {@User}", user);
        return Result.Ok(user);
    }

    public Result Update(User entity)
    {
        logger.Information("Atualizando usuário: {@User}", entity);
        if (entity == null)
        {
            logger.Warning("Usuário nulo ao atualizar.");
            return Result.Fail("Usuário nulo.");
        }

        User? existing = userRepository.GetById(entity.Id);
        if (existing == null)
        {
            logger.Warning("Usuário não encontrado para atualização: {Id}", entity.Id);
            return Result.Fail("Usuário não encontrado.");
        }

        userRepository.Update(entity);
        logger.Information("Usuário atualizado com sucesso: {@User}", entity);
        return Result.Ok();
    }
}