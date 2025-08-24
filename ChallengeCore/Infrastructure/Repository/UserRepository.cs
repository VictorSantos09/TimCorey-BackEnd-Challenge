using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;
internal class UserRepository(AppDbContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public void Add(User user)
    {
        logger.LogInformation("Adicionando usuário: {@User}", user);
        context.Users.Add(user);
        context.SaveChanges();
        logger.LogInformation("Usuário adicionado com sucesso: {@User}", user);
    }

    public void Delete(User entity)
    {
        logger.LogInformation("Removendo usuário: {@User}", entity);
        context.Users.Remove(entity);
        context.SaveChanges();
        logger.LogInformation("Usuário removido com sucesso: {@User}", entity);
    }

    public User? Get(string email)
    {
        logger.LogInformation("Buscando usuário por email: {Email}", email);
        var user = context.Users.FirstOrDefault(x => x.Email == email);
        if (user != null)
            logger.LogInformation("Usuário encontrado: {@User}", user);
        else
            logger.LogWarning("Usuário não encontrado para o email: {Email}", email);
        return user;
    }

    public IEnumerable<User> GetAll()
    {
        logger.LogInformation("Buscando todos os usuários.");
        var users = context.Users.ToList();
        logger.LogInformation("Total de usuários encontrados: {Count}", users.Count);
        return users;
    }

    public User? GetById(int id)
    {
        logger.LogInformation("Buscando usuário por Id: {Id}", id);
        var user = context.Users.Find(id);
        if (user != null)
            logger.LogInformation("Usuário encontrado: {@User}", user);
        else
            logger.LogWarning("Usuário não encontrado para o Id: {Id}", id);
        return user;
    }

    public void Update(User entity)
    {
        logger.LogInformation("Atualizando usuário: {@User}", entity);
        context.Users.Update(entity);
        context.SaveChanges();
        logger.LogInformation("Usuário atualizado com sucesso: {@User}", entity);
    }
}
