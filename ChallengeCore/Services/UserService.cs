using ChallengeCore.DTOs;
using ChallengeCore.Models;

namespace ChallengeCore.Services;
public class UserService
{
    private readonly UserDAO _userDAO;

    public UserService()
    {
        _userDAO = new();
    }

    public async Task<BaseDTO> Register(UserDTO dto)
    {
        try
        {
            if (dto.Name.Length < 3)
                return BaseDTO.Invalid("nome deve conter no mínimo 3 dígitos.");

            if (!dto.Email.Contains('@') || dto.Email.Length < 3)
                return BaseDTO.Invalid("email deve conter '@' e no mínimo 3 dígitos.");

            if (dto.Nickname.Length < 3)
                return BaseDTO.Invalid("nickname deve conter no mínimo 3 dígitos.");

            return await _userDAO.Create(dto);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<BaseDTO> ViewInfo(string email, string nickname)
    {
        try
        {
            UserDTO? result = await _userDAO.Get(email, nickname);
            return result == null ? BaseDTO.Invalid("usuário não encontrado") : BaseDTO.Valid("Usuário encontrado", result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}