using MySql.Data.MySqlClient;

namespace ChallengeCore.Data;
internal class DatabaseConnection
{
    private readonly string _connectionString = "Server=localhost;Database=timchallenge;Uid=root;Pwd=root;";
    protected MySqlConnection _connection;

    protected MySqlConnection Connect()
    {
        try
        {
            _connection = new MySqlConnection(_connectionString);
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