using ChallengeCore.Domain.Models;
using ChallengeCore.Shared;

namespace ChallengeCore.Infrastructure.Repository.Abstractions;
public interface IUserRepository : IRepository<User>
{
    User? Get(string email);
}
