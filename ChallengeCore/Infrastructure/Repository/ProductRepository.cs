using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;
internal class ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    public IEnumerable<Product> GetAll()
    {
        logger.Information("Buscando todos os produtos.");

        IEnumerable<Product> products = [.. context.Products
            .Include(x => x.PurchaseItems)];

        logger.Information("Total de produtos encontrados: {Count}", products.Count());
        return products;
    }

    public Product? GetById(int id)
    {
        logger.Information("Buscando produto por Id: {Id}", id);

        Product? product = context.Products
            .Include(x => x.PurchaseItems)
            .FirstOrDefault(x => x.Id == id);

        if (product != null)
        {
            logger.Information("Produto encontrado: {Product}", product);
        }
        else
        {
            logger.Warning("Produto não encontrado para o Id: {Id}", id);
        }

        return product;
    }

    public void Add(Product entity)
    {
        logger.Information("Adicionando produto: {Product}", entity);
        _ = context.Products.Add(entity);
        _ = context.SaveChanges();
        logger.Information("Produto adicionado com sucesso: {Product}", entity);
    }

    public void Update(Product entity)
    {
        logger.Information("Atualizando produto: {Product}", entity);
        _ = context.Products.Update(entity);
        _ = context.SaveChanges();
        logger.Information("Produto atualizado com sucesso: {Product}", entity);
    }

    public void Delete(Product entity)
    {
        logger.Information("Removendo produto: {Product}", entity);
        _ = context.Products.Remove(entity);
        _ = context.SaveChanges();
        logger.Information("Produto removido com sucesso: {Product}", entity);
    }
}
