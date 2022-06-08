using System.Collections;
using System.Data.SqlClient;
using ADO;
using ADO.Net;
using ADO.Net.Client;
using EVT_Server.Models;
using EVT_Server.Models.Election;
using Npgsql;

namespace EVT_Server.Repository{

public class ElectionRepository
{
    private readonly DatabaseConnection? _db;
    public ElectionRepository()
    {
        _db = DatabaseConnection.GetInstance();
    }


    public List<ElectionInfo>? GetAllElections()
    {
        string sqlQuery = "SELECT * FROM elections";

        var dataReader = _db.ExecuteReadCommand(sqlQuery);

        List<ElectionInfo> elections = new List<ElectionInfo>();

        while (dataReader.Read())
        {
            ElectionInfo election = ReadElection(dataReader);
            elections.Add(election);
        }
        
        
        return elections;
    }

    private static ElectionInfo ReadElection(NpgsqlDataReader dataReader)
    {
        Guid id = dataReader["id"] as Guid? ?? default;
        string? electionName = dataReader["name"] as string;

        ElectionInfo electionInfo = new ElectionInfo()
        {
            Id = id,
            ElectionName = electionName ?? string.Empty
        };

        return electionInfo;
    }


    public bool InsertElection(ElectionInfo election)
    {
        string sqlQuery = $"INSERT INTO elections (id,name) VALUES ('{election.Id}', '{election.ElectionName}');";

        var affectedRows = _db.ExecuteWriteCommand(sqlQuery);
        
        if (affectedRows == 0)
        {
            return false;
        }

        CandidateRepository candidateRepository = new CandidateRepository();
        
        foreach (var candidateOption in election.Options)
        {
            candidateRepository.InsertElection(candidateOption, election.Id);
        }
        
        return true;
    }
    
    
}
}