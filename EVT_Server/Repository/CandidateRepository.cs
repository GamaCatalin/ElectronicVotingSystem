using EVT_Server.Models.Election;
using Npgsql;

namespace EVT_Server.Repository;

public class CandidateRepository
{
    private readonly DatabaseConnection? _db;

    public CandidateRepository()
    {
        _db  = DatabaseConnection.GetInstance();
    }
    
    public async Task<IEnumerable<CandidateOption>?> GetAll()
    {
        string sqlQuery = "SELECT * FROM elections";

        var dataReader = await _db.ExecuteReadCommand(sqlQuery);

        Console.WriteLine((await dataReader.GetColumnSchemaAsync()).ToString());
        
        return dataReader.GetEnumerator() as IEnumerable<CandidateOption>;
    }


    public async Task<bool> InsertElection(CandidateOption candidate, Guid electionId)
    {
        string sqlQuery = $"INSERT INTO candidates (id,title,address,picture,party,vote_count,election_id) VALUES " + 
                          $" ('{candidate.Id}','{candidate.Title}','{candidate.Address}','{candidate.Picture}','{candidate.Party}','{candidate.VoteCount}','{electionId}');";

        var affectedRows = await _db.ExecuteWriteCommand(sqlQuery);
        
        if (affectedRows == 0)
        {
            return false;
        }
        
        return true;
    }

    public async Task<bool> IncrementVote(Guid candidateId)
    {
        var affectedRows = await _db.ExecuteIncrementProcedure(candidateId);

        if (affectedRows == 0)
        {
            return false;
        }

        return true;
    }
    
}