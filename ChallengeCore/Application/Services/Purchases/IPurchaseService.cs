using ChallengeCore.Domain.Models;
using ChallengeCore.Shared;

namespace ChallengeCore.Application.Services.Purchases;

public interface IPurchaseService : IService<Purchase>
{
    IEnumerable<Purchase> GetUserPurchases(string email);
}