using ChallengeCore.DTOs;

namespace ChallengeCore.Models;
public interface IUserDAO
{
    Task<BaseDTO> Create(UserDTO dto);
    Task<UserDTO?> Get(string email, string nickname);
}