using ChallengeCore.Data;
using ChallengeCore.DTOs;
using ChallengeCore.SQLModels;
using Dapper;

namespace ChallengeCore.Models;
public class UserDAO : DatabaseConnection, IUserDAO
{
    public async Task<BaseDTO> Create(UserDTO dto)
    {
        try
        {
            using (_connection = Connect())
            {
                var result = await _connection.QueryAsync<UserSQL>("SELECT US_EMAIL, US_NICKNAME FROM users WHERE US_EMAIL = @Email OR US_NICKNAME = @Nickname",
                    new
                    {
                        Email = dto.Email.ToUpper(),
                        Nickname = dto.Nickname.ToUpper()
                    });

                if (result.Any())
                    return BaseDTO.Invalid("usuário já existente");

                _ = await _connection.ExecuteAsync("INSERT INTO users (US_NAME, US_EMAIL, US_NICKNAME) VALUES (@Name, @Email, @Nickname)",
                    new
                    {
                        Name = dto.Name.ToUpper(),
                        Email = dto.Email.ToUpper(),
                        Nickname = dto.Nickname.ToUpper()
                    });
            }
            return BaseDTO.Valid("conta criada com sucesso");
        }
        catch (Exception)
        {
            throw;
        }
        finally { Disconnect(); }
    }

    public async Task<UserDTO?> Get(string email, string nickname)
    {
        try
        {
            using (_connection = Connect())
            {
                var resultSQL = await _connection.QueryAsync<UserSQL>("SELECT US_NAME, US_EMAIL, US_NICKNAME FROM users WHERE US_EMAIL = @Email AND US_NICKNAME = @Nickname",
                    new
                    {
                        Email = email.ToUpper(),
                        Nickname = nickname
                    });
                var first = resultSQL.FirstOrDefault();

                return first is null ? null : new UserDTO()
                {
                    Name = first.US_NAME,
                    Email = first.US_EMAIL,
                    Nickname = first.US_NICKNAME
                };
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally { Disconnect(); }
    }
}