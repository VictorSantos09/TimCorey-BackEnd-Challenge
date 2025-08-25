using Ardalis.GuardClauses;

namespace ChallengeCore.Domain.Models;

public class PurchaseItem
{
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public Purchase Purchase { get; set; }
    public Product Product { get; set; }

    public PurchaseItem(int id, int purchaseId, int productId, int quantity, Purchase purchase, Product product)
    {
        Id = Guard.Against.NegativeOrZero(id);
        PurchaseId = Guard.Against.NegativeOrZero(purchaseId);
        ProductId = Guard.Against.NegativeOrZero(productId);
        Quantity = Guard.Against.NegativeOrZero(quantity);
        Purchase = Guard.Against.Null(purchase);
        Product = Guard.Against.Null(product);
    }

    public PurchaseItem()
    {

    }
}
