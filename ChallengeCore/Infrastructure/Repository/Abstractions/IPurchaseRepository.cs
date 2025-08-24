using ChallengeCore.Domain.Models;
using ChallengeCore.Shared;

namespace ChallengeCore.Infrastructure.Repository.Abstractions;

public interface IPurchaseRepository : IRepository<Purchase>
{
    IEnumerable<Purchase> GetUserPurchases(string email);
}
