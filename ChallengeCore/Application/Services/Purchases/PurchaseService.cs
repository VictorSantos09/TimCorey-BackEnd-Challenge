using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Purchases;
internal class PurchaseService(IPurchaseRepository purchaseRepository, ILogger<PurchaseService> logger) : IPurchaseService
{
    public Result<Purchase> GetById(int id)
    {
        logger.Information("Buscando compra por Id: {Id}", id);
        Purchase? purchase = purchaseRepository.GetById(id);
        if (purchase is null)
        {
            logger.Warning("Compra não encontrada para o Id: {Id}", id);
            return Result.Fail<Purchase>("Purchase not found.");
        }
        logger.Information("Compra encontrada: {@Purchase}", purchase);
        return Result.Ok(purchase);
    }

    public Result<IEnumerable<Purchase>> GetAll()
    {
        logger.Information("Buscando todas as compras.");
        IEnumerable<Purchase> purchases = purchaseRepository.GetAll();
        logger.Information("Total de compras encontradas: {Count}", purchases.Count());
        return Result.Ok(purchases);
    }

    public Result Add(Purchase purchase)
    {
        logger.Information("Adicionando compra: {@Purchase}", purchase);
        if (purchase is null)
        {
            logger.Warning("Compra nula ao adicionar.");
            return Result.Fail("Purchase is null.");
        }


        purchaseRepository.Add(purchase);
        logger.Information("Compra adicionada com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public Result Update(Purchase purchase)
    {
        logger.Information("Atualizando compra: {@Purchase}", purchase);
        if (purchase is null)
        {
            logger.Warning("Compra nula ao atualizar.");
            return Result.Fail("Purchase is null.");
        }

        Purchase? existing = purchaseRepository.GetById(purchase.Id);
        if (existing is null)
        {
            logger.Warning("Compra não encontrada para atualização: {Id}", purchase.Id);
            return Result.Fail("Purchase not found.");
        }


        purchaseRepository.Update(purchase);
        logger.Information("Compra atualizada com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.Information("Removendo compra por Id: {Id}", id);
        Purchase? purchase = purchaseRepository.GetById(id);
        if (purchase is null)
        {
            logger.Warning("Compra não encontrada para remoção: {Id}", id);
            return Result.Fail("Purchase not found.");
        }

        purchaseRepository.Delete(purchase);
        logger.Information("Compra removida com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public IEnumerable<Purchase> GetUserPurchases(string email)
    {
        logger.Information("Buscando compras de produtos do usuário: {Email}", email);
        IEnumerable<Purchase> purchases = purchaseRepository.GetUserPurchases(email);
        logger.Information("Total de produtos encontrados para o usuário {Email}: {Count}", email, purchases.Count());
        return purchases;
    }
}
