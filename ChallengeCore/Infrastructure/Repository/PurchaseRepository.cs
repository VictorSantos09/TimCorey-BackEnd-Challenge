using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Infrastructure.Repository;

internal class PurchaseRepository(AppDbContext context, ILogger<PurchaseRepository> logger) : IPurchaseRepository
{
    public void Add(Purchase entity)
    {
        logger.Information("Adicionando compra: {@Purchase}", entity);
        _ = context.Purchases.Add(entity);
        _ = context.SaveChanges();
        logger.Information("Compra adicionada com sucesso: {@Purchase}", entity);
    }

    public void Delete(Purchase entity)
    {
        logger.Information("Removendo compra: {@Purchase}", entity);
        _ = context.Purchases.Remove(entity);
        _ = context.SaveChanges();
        logger.Information("Compra removida com sucesso: {@Purchase}", entity);
    }

    public IEnumerable<Purchase> GetAll()
    {
        logger.Information("Buscando todas as compras.");
        List<Purchase> purchases = [.. context.Purchases
            .Include(p => p.User)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)];
        logger.Information("Total de compras encontradas: {Count}", purchases.Count);
        return purchases;
    }

    public Purchase? GetById(int id)
    {
        logger.Information("Buscando compra por Id: {Id}", id);
        Purchase? purchase = context.Purchases
            .Include(p => p.User)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(p => p.Id.Equals(id));
        if (purchase != null)
        {
            logger.Information("Compra encontrada: {@Purchase}", purchase);
        }
        else
        {
            logger.Warning("Compra não encontrada para o Id: {Id}", id);
        }

        return purchase;
    }

    public void Update(Purchase entity)
    {
        logger.Information("Atualizando compra: {@Purchase}", entity);
        _ = context.Purchases.Update(entity);
        _ = context.SaveChanges();
        logger.Information("Compra atualizada com sucesso: {@Purchase}", entity);
    }

    public IEnumerable<Purchase> GetUserPurchases(string email)
    {
        logger.Information("Buscando compras de produtos do usuário: {Email}", email);
        List<Purchase> products = [.. context.Purchases
            .Where(x => x.User.Email == email)
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(i => i.Product)];
        logger.Information("Total de produtos encontrados para o usuário {Email}: {Count}", email, products.Count);
        return products;
    }
}
