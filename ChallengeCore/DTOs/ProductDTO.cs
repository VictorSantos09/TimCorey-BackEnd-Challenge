namespace ChallengeCore.DTOs;
public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public ProductDTO(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public ProductDTO()
    {
        
    }
}