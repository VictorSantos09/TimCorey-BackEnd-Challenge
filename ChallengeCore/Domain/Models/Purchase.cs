using Ardalis.GuardClauses;

namespace ChallengeCore.Domain.Models;
public class Purchase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime PurchaseDate { get; set; }

    public User User { get; set; }
    public IEnumerable<PurchaseItem> Items { get; set; } = [];

    public Purchase(int userId, DateTime purchaseDate, User user, IEnumerable<PurchaseItem> items)
    {
        UserId = Guard.Against.NegativeOrZero(userId);
        PurchaseDate = Guard.Against.NullOrOutOfSQLDateRange(purchaseDate);
        User = Guard.Against.Null(user);
        Items = Guard.Against.NullOrEmpty(items);
    }

    public Purchase() { }
}
