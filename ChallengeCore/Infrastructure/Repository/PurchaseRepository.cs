using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;

internal class PurchaseRepository(AppDbContext context, ILogger<PurchaseRepository> logger) : IPurchaseRepository
{
    public void Add(Purchase entity)
    {
        logger.LogInformation("Adicionando compra: {@Purchase}", entity);
        context.Purchases.Add(entity);
        context.SaveChanges();
        logger.LogInformation("Compra adicionada com sucesso: {@Purchase}", entity);
    }

    public void Delete(Purchase entity)
    {
        logger.LogInformation("Removendo compra: {@Purchase}", entity);
        context.Purchases.Remove(entity);
        context.SaveChanges();
        logger.LogInformation("Compra removida com sucesso: {@Purchase}", entity);
    }

    public IEnumerable<Purchase> GetAll()
    {
        logger.LogInformation("Buscando todas as compras.");
         var purchases = context.Purchases
             .Include(p => p.User)
             .Include(p => p.Items)
             .ThenInclude(i => i.Product)
             .ToList();
        logger.LogInformation("Total de compras encontradas: {Count}", purchases.Count());
        return purchases;
    }

    public Purchase? GetById(int id)
    {
        logger.LogInformation("Buscando compra por Id: {Id}", id);
        var purchase = context.Purchases
            .Include(p => p.User)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(p => p.Id.Equals(id));
        if (purchase != null)
            logger.LogInformation("Compra encontrada: {@Purchase}", purchase);
        else
            logger.LogWarning("Compra não encontrada para o Id: {Id}", id);
        return purchase;
    }

    public void Update(Purchase entity)
    {
        logger.LogInformation("Atualizando compra: {@Purchase}", entity);
        context.Purchases.Update(entity);
        context.SaveChanges();
        logger.LogInformation("Compra atualizada com sucesso: {@Purchase}", entity);
    }

    public IEnumerable<Purchase> GetUserPurchases(string email)
    {
        logger.LogInformation("Buscando compras de produtos do usuário: {Email}", email);
        var products = context.Purchases
            .Where(x => x.User.Email == email)
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(i => i.Product)
            .ToList();
        logger.LogInformation("Total de produtos encontrados para o usuário {Email}: {Count}", email, products.Count);
        return products;
    }
}
