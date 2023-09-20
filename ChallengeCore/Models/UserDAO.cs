using ChallengeCore.Data;
using ChallengeCore.DTOs;

namespace ChallengeCore.Models;
internal class UserDAO : DatabaseConnection
{
    public async Task<bool> Create(UserDTO dto)
    {
        try
        {
            _connection = Connect();
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "insert into users (US_NAME, US_EMAIL, US_PASSWORD) values (?,?,?)";
            cmd.Parameters.AddWithValue("US_NAME", dto.Name.ToUpper());
            cmd.Parameters.AddWithValue("US_EMAIL", dto.Email.ToUpper());
            cmd.Parameters.AddWithValue("US_PASSWORD", dto.Password);
            await cmd.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally { Disconnect(); }
    }

    public async Task<UserDTO?> Get(string email, string password)
    {
        try
        {
            _connection = Connect();
            _connection.Open();
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "select US_NAME, US_EMAIL from USERS where US_EMAIL = @email and US_PASSWORD = @password";
            cmd.Parameters.AddWithValue("@email", email.ToUpper());
            cmd.Parameters.AddWithValue("@password", password);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return new UserDTO(reader.GetString(0), reader.GetString(1), null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally { Disconnect(); }
    }
}