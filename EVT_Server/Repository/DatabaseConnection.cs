using System.Data;
using Npgsql;

namespace EVT_Server.Repository{

public class DatabaseConnection
{
    private string _connectionString = "Host=localhost;Username=postgres;Password=123;Database=EVS_TestDB";
    private static DatabaseConnection? _instance;


    private DatabaseConnection()
    {
      
    }

    public static DatabaseConnection GetInstance()
    {
        if (_instance is null)
        {
            _instance = new DatabaseConnection();
        }
        
        return _instance;
    }


    public NpgsqlDataReader ExecuteReadCommand(string sqlCommand)
    {
        var databaseConnection = new NpgsqlConnection(_connectionString);
        databaseConnection.Open();  
        
        var command = new NpgsqlCommand(sqlCommand, databaseConnection);

        var dataReader = command.ExecuteReader();

        return dataReader;
    }
    
    
    public int ExecuteWriteCommand(string sqlCommand)
    {
        var databaseConnection = new NpgsqlConnection(_connectionString);
        databaseConnection.Open();  
        
        var command = new NpgsqlCommand(sqlCommand, databaseConnection);

        var affectedRows = command.ExecuteNonQuery();

        return affectedRows;
    }

    public int ExecuteIncrementProcedure(Guid candidateId)
    {
        var databaseConnection = new NpgsqlConnection(_connectionString);
        databaseConnection.Open();  
        
        var incrementProcedure = "call increment_vote(:incremented_id)";
        var command = new NpgsqlCommand(incrementProcedure, databaseConnection);
        command.CommandType = CommandType.Text;
        command.Parameters.AddWithValue("incremented_id", candidateId);

        var affectedRows = command.ExecuteNonQuery();
        
        return affectedRows;
    }
}
}