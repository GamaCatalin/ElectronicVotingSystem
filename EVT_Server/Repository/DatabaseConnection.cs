using System.Data;
using Npgsql;

namespace EVT_Server.Repository;

public class DatabaseConnection
{
    private string _connectionString = "Host=localhost;Username=postgres;Password=123;Database=EVS_TestDB";
    private readonly NpgsqlConnection _databaseConnection;
    private static DatabaseConnection? _instance;


    private DatabaseConnection()
    {
        _databaseConnection = new NpgsqlConnection(_connectionString);
        _databaseConnection.Open();        
    }

    public static DatabaseConnection GetInstance()
    {
        if (_instance is null)
        {
            _instance = new DatabaseConnection();
        }
        
        return _instance;
    }

    public void Deconstruct()
    {
        _databaseConnection.Close();
    }


    public async Task<NpgsqlDataReader> ExecuteReadCommand(string sqlCommand)
    {
        var command = new NpgsqlCommand(sqlCommand, _databaseConnection);

        var dataReader = await command.ExecuteReaderAsync();

        return dataReader;
    }
    
    
    public async Task<int> ExecuteWriteCommand(string sqlCommand)
    {
        var command = new NpgsqlCommand(sqlCommand, _databaseConnection);

        var affectedRows = await command.ExecuteNonQueryAsync();

        return affectedRows;
    }

    public async Task<int> ExecuteIncrementProcedure(Guid candidateId)
    {
        var incrementProcedure = "call increment_vote(:incremented_id)";
        var command = new NpgsqlCommand(incrementProcedure, _databaseConnection);
        command.CommandType = CommandType.Text;
        command.Parameters.AddWithValue("incremented_id", candidateId);

        var affectedRows = await command.ExecuteNonQueryAsync();

        return affectedRows;
    }
}