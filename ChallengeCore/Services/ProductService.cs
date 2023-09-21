using ChallengeCore.DTOs;
using ChallengeCore.Models;

namespace ChallengeCore.Services;
public class ProductService
{
    private readonly ProductDAO _dao = new();

    public async Task<BaseDTO> GetUserProducts(string email)
    {
        var result = await _dao.GetUserProducts(email);
        return result.Count() <= 0 ? BaseDTO.Valid("Nenhum produto encontrado") : BaseDTO.Valid("produtos encontrados", result);
    } 

    public async Task<BaseDTO> Buy(BuyProductDTO dto)
    {
        var result = await _dao.AddBoughtProduct(dto);
        return result;
    }

    public async Task<BaseDTO> ViewAll()
    {
        return BaseDTO.Valid("buscado todos os produtos", await _dao.GetAll());
    }
}