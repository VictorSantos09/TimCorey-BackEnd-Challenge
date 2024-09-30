using ChallengeCore.DTOs;

namespace ChallengeCore.Services;
public interface IUserService
{
    Task<BaseDTO> Register(UserDTO dto);
    Task<BaseDTO> ViewInfo(string email, string nickname);
}