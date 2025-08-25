using ChallengeCore.Domain.Models;
using ChallengeCore.Shareed;

namespace ChallengeCore.Infrastructure.Repository.Abstractions;
public interface IUserRepository : IRepository<User>
{
    User? GetByEmail(string email);
}
