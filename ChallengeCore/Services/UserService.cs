using ChallengeCore.DTOs;
using ChallengeCore.Models;
using System.ComponentModel.DataAnnotations;

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
            if (ModelValidatorService.Validate(dto, out List<ValidationResult> errors) == false)
                return BaseDTO.Invalid("Dados inválidos fornecidos", errors);

            await _userDAO.Create(dto);
            return BaseDTO.Valid("conta criada com sucesso");
        }
        catch (Exception)
        {
            return BaseDTO.Invalid("Não foi possível registrar o usuário");
        }
    }

    public async Task<BaseDTO> ViewInfo(string email, string password)
    {
        try
        {
            var result = await _userDAO.Get(email, password);
            if (result == null)
                return BaseDTO.Invalid("usuário não encontrado");

            else
                return BaseDTO.Valid("Usuário encontrado", result);
        }
        catch (Exception)
        {
            return BaseDTO.Invalid("Não foi possível ver as informações do usuário");
        }
    }
}