using ChallengeCore.Data;
using ChallengeCore.DTOs;
using ChallengeCore.SQLModels;
using Dapper;

namespace ChallengeCore.Models;
internal class UserDAO : DatabaseConnection
{
    public async Task<bool> Create(UserDTO dto)
    {
        try
        {
            using (_connection = Connect())
            {
                await _connection.ExecuteAsync("insert into users (US_NAME, US_EMAIL, US_PASSWORD) VALUES(@Name, @Email, @Password)",
                    new { Name = dto.Name.ToUpper(), Email = dto.Email.ToUpper(), dto.Password });
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally { Disconnect(); }
    }

    public async Task<UserDTO?> Get(string email, string password)
    {
        try
        {
            using (_connection = Connect())
            {
                var result = await _connection.QueryFirstAsync<UserSQL>("select US_NAME, US_EMAIL FROM users WHERE US_EMAIL = @Email AND US_PASSWORD = @Password",
                    new { Email = email.ToUpper(), Password = password });

                return result is null ? null : new UserDTO() { Name = result.US_NAME, Email = result.US_EMAIL };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        finally { Disconnect(); }
    }
}