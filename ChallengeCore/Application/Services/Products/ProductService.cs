using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Products;
public class ProductService(IProductRepository repository, ILogger<ProductService> logger) : IProductService
{
    public Result<IEnumerable<Product>> GetAll()
    {
        logger.Information("Buscando todos os produtos.");
        IEnumerable<Product> products = repository.GetAll();
        logger.Information("Total de produtos encontrados: {Count}", products.Count());
        return Result.Ok(products);
    }

    public Result<Product> GetById(int id)
    {
        logger.Information("Buscando produto por Id: {Id}", id);
        Product? product = repository.GetById(id);
        if (product == null)
        {
            logger.Warning("Produto não encontrado para o Id: {Id}", id);
            return Result.Fail<Product>("Produto não encontrado");
        }
        logger.Information("Produto encontrado: {@Product}", product);
        return Result.Ok(product);
    }

    public Result Add(Product entity)
    {
        logger.Information("Adicionando produto: {@Product}", entity);

        repository.Add(entity);
        logger.Information("Produto adicionado com sucesso: {@Product}", entity);
        return Result.Ok();
    }

    public Result Update(Product entity)
    {
        logger.Information("Atualizando produto: {@Product}", entity);

        repository.Update(entity);
        logger.Information("Produto atualizado com sucesso: {@Product}", entity);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.Information("Removendo produto por Id: {Id}", id);

        Product? product = repository.GetById(id);

        if (product == null)
        {
            logger.Warning("Produto não encontrado para remoção, Id: {Id}", id);
            return Result.Fail("Produto não encontrado");
        }

        repository.Delete(product);
        logger.Information("Produto removido com sucesso: {Id}", id);
        return Result.Ok();
    }
}
