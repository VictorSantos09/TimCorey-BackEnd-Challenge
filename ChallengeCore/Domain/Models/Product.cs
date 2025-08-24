using Ardalis.GuardClauses;

namespace ChallengeCore.Domain.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public IEnumerable<PurchaseItem> PurchaseItems { get; set; } = [];

    public Product(int id, string name, decimal price, IEnumerable<PurchaseItem> purchaseItems)
    {
        Id = Guard.Against.NegativeOrZero(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Price = Guard.Against.NegativeOrZero(price, nameof(price));
        PurchaseItems = Guard.Against.NullOrEmpty(purchaseItems, nameof(purchaseItems));
    }

    public Product()
    {
        
    }
}
