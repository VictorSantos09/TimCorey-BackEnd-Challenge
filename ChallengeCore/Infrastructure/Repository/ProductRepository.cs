using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;
internal class ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    public IEnumerable<Product> GetAll()
    {
        logger.LogInformation("Buscando todos os produtos.");
        
        IEnumerable<Product> products = [.. context.Products
            .Include(x => x.PurchaseItems)];
        
        logger.LogInformation("Total de produtos encontrados: {Count}", products.Count());
        return products;
    }

    public Product? GetById(int id)
    {
        logger.LogInformation("Buscando produto por Id: {Id}", id);
        var product = context.Products
            .Include(x => x.PurchaseItems)
            .FirstOrDefault(x => x.Id == id);
        
        if (product != null)
            logger.LogInformation("Produto encontrado: {@Product}", product);
        else
            logger.LogWarning("Produto não encontrado para o Id: {Id}", id);
        return product;
    }

    public void Add(Product entity)
    {
        logger.LogInformation("Adicionando produto: {@Product}", entity);
        context.Products.Add(entity);
        context.SaveChanges();
        logger.LogInformation("Produto adicionado com sucesso: {@Product}", entity);
    }

    public void Update(Product entity)
    {
        logger.LogInformation("Atualizando produto: {@Product}", entity);
        context.Products.Update(entity);
        context.SaveChanges();
        logger.LogInformation("Produto atualizado com sucesso: {@Product}", entity);
    }

    public void Delete(Product entity)
    {
        logger.LogInformation("Removendo produto: {@Product}", entity);
        context.Products.Remove(entity);
        context.SaveChanges();
        logger.LogInformation("Produto removido com sucesso: {@Product}", entity);
    }
}
