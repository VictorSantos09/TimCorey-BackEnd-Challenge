using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Products;
public class ProductService(IProductRepository repository, ILogger<ProductService> logger) : IProductService
{
    public Result<IEnumerable<Product>> GetAll()
    {
        logger.LogInformation("Buscando todos os produtos.");
        var products = repository.GetAll();
        logger.LogInformation("Total de produtos encontrados: {Count}", products.Count());
        return Result.Ok(products);
    }

    public Result<Product> GetById(int id)
    {
        logger.LogInformation("Buscando produto por Id: {Id}", id);
        var product = repository.GetById(id);
        if (product == null)
        {
            logger.LogWarning("Produto não encontrado para o Id: {Id}", id);
            return Result.Fail<Product>("Produto não encontrado");
        }
        logger.LogInformation("Produto encontrado: {@Product}", product);
        return Result.Ok(product);
    }

    public Result Add(Product entity)
    {
        logger.LogInformation("Adicionando produto: {@Product}", entity);

        repository.Add(entity);
        logger.LogInformation("Produto adicionado com sucesso: {@Product}", entity);
        return Result.Ok();
    }

    public Result Update(Product entity)
    {
        logger.LogInformation("Atualizando produto: {@Product}", entity);

        repository.Update(entity);
        logger.LogInformation("Produto atualizado com sucesso: {@Product}", entity);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.LogInformation("Removendo produto por Id: {Id}", id);
        
        var product = repository.GetById(id);

        if (product == null)
        {
            logger.LogWarning("Produto não encontrado para remoção, Id: {Id}", id);
            return Result.Fail("Produto não encontrado");
        }

        repository.Delete(product);
        logger.LogInformation("Produto removido com sucesso: {Id}", id);
        return Result.Ok();
    }
}