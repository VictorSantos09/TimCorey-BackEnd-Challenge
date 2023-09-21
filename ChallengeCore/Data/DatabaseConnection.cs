using MySql.Data.MySqlClient;
using System.Data;

namespace ChallengeCore.Data;
internal class DatabaseConnection
{
    private readonly string _connectionString = "Server=localhost;Database=testing;Uid=root;Pwd=root;";
    protected IDbConnection _connection;

    protected IDbConnection Connect()
    {
        try
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
            return _connection;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    protected void Disconnect()
    {
        _connection.Close();
    }
}