using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;
internal class UserRepository(AppDbContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public void Add(User user)
    {
        logger.Information("Adicionando usuário: {@User}", user);
        _ = context.Users.Add(user);
        _ = context.SaveChanges();
        logger.Information("Usuário adicionado com sucesso: {@User}", user);
    }

    public void Delete(User entity)
    {
        logger.Information("Removendo usuário: {@User}", entity);
        _ = context.Users.Remove(entity);
        _ = context.SaveChanges();
        logger.Information("Usuário removido com sucesso: {@User}", entity);
    }

    public User? GetByEmail(string email)
    {
        logger.Information("Buscando usuário por email: {Email}", email);
        User? user = context.Users.FirstOrDefault(x => x.Email == email);
        if (user != null)
        {
            logger.Information("Usuário encontrado: {@User}", user);
        }
        else
        {
            logger.Warning("Usuário não encontrado para o email: {Email}", email);
        }

        return user;
    }

    public IEnumerable<User> GetAll()
    {
        logger.Information("Buscando todos os usuários.");
        List<User> users = context.Users.ToList();
        logger.Information("Total de usuários encontrados: {Count}", users.Count);
        return users;
    }

    public User? GetById(int id)
    {
        logger.Information("Buscando usuário por Id: {Id}", id);
        User? user = context.Users.Find(id);
        if (user != null)
        {
            logger.Information("Usuário encontrado: {@User}", user);
        }
        else
        {
            logger.Warning("Usuário não encontrado para o Id: {Id}", id);
        }

        return user;
    }

    public void Update(User entity)
    {
        logger.Information("Atualizando usuário: {@User}", entity);
        _ = context.Users.Update(entity);
        _ = context.SaveChanges();
        logger.Information("Usuário atualizado com sucesso: {@User}", entity);
    }
}
