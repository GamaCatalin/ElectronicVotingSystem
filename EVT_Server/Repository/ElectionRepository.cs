using System.Collections;
using System.Data.SqlClient;
using ADO;
using ADO.Net;
using ADO.Net.Client;
using EVT_Server.Models;
using Npgsql;

namespace EVT_Server.Repository;

public class ElectionRepository
{
    private readonly DatabaseConnection? _db;
    public ElectionRepository()
    {
        _db = DatabaseConnection.GetInstance();
    }


    public async Task<IEnumerable<ElectionInfo>?> GetAll()
    {
        string sqlQuery = "SELECT * FROM elections";

        var dataReader = await _db.ExecuteReadCommand(sqlQuery);

        Console.WriteLine((await dataReader.GetColumnSchemaAsync()).ToString());
        
        return dataReader.GetEnumerator() as IEnumerable<ElectionInfo>;
    }


    public async Task<bool> InsertElection(ElectionInfo election)
    {
        string sqlQuery = $"INSERT INTO elections (id,name) VALUES ('{election.Id}', '{election.ElectionName}');";

        var affectedRows = await _db.ExecuteWriteCommand(sqlQuery);
        
        if (affectedRows == 0)
        {
            return false;
        }

        CandidateRepository candidateRepository = new CandidateRepository();
        
        foreach (var candidateOption in election.Options)
        {
            await candidateRepository.InsertElection(candidateOption, election.Id);
        }
        
        return true;
    }
    
    
}