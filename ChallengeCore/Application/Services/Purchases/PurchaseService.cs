using ChallengeCore.Domain.Models;
using ChallengeCore.Infrastructure.Repository.Abstractions;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ChallengeCore.Application.Services.Purchases;
internal class PurchaseService(IPurchaseRepository purchaseRepository, ILogger<PurchaseService> logger) : IPurchaseService
{
    public Result<Purchase> GetById(int id)
    {
        logger.LogInformation("Buscando compra por Id: {Id}", id);
        var purchase = purchaseRepository.GetById(id);
        if (purchase is null)
        {
            logger.LogWarning("Compra não encontrada para o Id: {Id}", id);
            return Result.Fail<Purchase>("Purchase not found.");
        }
        logger.LogInformation("Compra encontrada: {@Purchase}", purchase);
        return Result.Ok(purchase);
    }

    public Result<IEnumerable<Purchase>> GetAll()
    {
        logger.LogInformation("Buscando todas as compras.");
        var purchases = purchaseRepository.GetAll();
        logger.LogInformation("Total de compras encontradas: {Count}", purchases.Count());
        return Result.Ok(purchases);
    }

    public Result Add(Purchase purchase)
    {
        logger.LogInformation("Adicionando compra: {@Purchase}", purchase);
        if (purchase is null)
        {
            logger.LogWarning("Compra nula ao adicionar.");
            return Result.Fail("Purchase is null.");
        }


        purchaseRepository.Add(purchase);
        logger.LogInformation("Compra adicionada com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public Result Update(Purchase purchase)
    {
        logger.LogInformation("Atualizando compra: {@Purchase}", purchase);
        if (purchase is null)
        {
            logger.LogWarning("Compra nula ao atualizar.");
            return Result.Fail("Purchase is null.");
        }

        var existing = purchaseRepository.GetById(purchase.Id);
        if (existing is null)
        {
            logger.LogWarning("Compra não encontrada para atualização: {Id}", purchase.Id);
            return Result.Fail("Purchase not found.");
        }


        purchaseRepository.Update(purchase);
        logger.LogInformation("Compra atualizada com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public Result Delete(int id)
    {
        logger.LogInformation("Removendo compra por Id: {Id}", id);
        var purchase = purchaseRepository.GetById(id);
        if (purchase is null)
        {
            logger.LogWarning("Compra não encontrada para remoção: {Id}", id);
            return Result.Fail("Purchase not found.");
        }

        purchaseRepository.Delete(purchase);
        logger.LogInformation("Compra removida com sucesso: {@Purchase}", purchase);
        return Result.Ok();
    }

    public IEnumerable<Purchase> GetUserPurchases(string email)
    {
        logger.LogInformation("Buscando compras de produtos do usuário: {Email}", email);
        var purchases = purchaseRepository.GetUserPurchases(email);
        logger.LogInformation("Total de produtos encontrados para o usuário {Email}: {Count}", email, purchases.Count());
        return purchases;
    }
}
