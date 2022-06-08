using System.Collections;
using EVT_Server.Models.Election;
using Npgsql;

namespace EVT_Server.Repository{

public class CandidateRepository
{
    private readonly DatabaseConnection? _db;

    public CandidateRepository()
    {
        _db  = DatabaseConnection.GetInstance();
    }
    
    public List<CandidateOption>? GetAll()
    {
        string sqlQuery = "SELECT * FROM candidates";

        var dataReader = _db.ExecuteReadCommand(sqlQuery);

        List<CandidateOption> candidates = new List<CandidateOption>();

        while (dataReader.Read())
        {
            CandidateOption candidate = ReadCandidate(dataReader);
            candidates.Add(candidate);
        }
        
        return candidates;
    }

    private CandidateOption ReadCandidate(NpgsqlDataReader dataReader)
    {
        Guid id = dataReader["id"] as Guid? ?? default;
        string? title = dataReader["title"] as string;
        string? address = dataReader["address"] as string;
        string? picture = dataReader["picture"] as string;
        string? party = dataReader["party"] as string;
        int voteCount = dataReader["vote_count"] as int? ?? 0;
        Guid electionId = dataReader["election_id"] as Guid? ?? default;
        

        CandidateOption electionInfo = new CandidateOption()
        {
            Id = id,
            Title = title ?? string.Empty,
            Address = address ?? string.Empty,
            Party = party ?? string.Empty,
            Picture = picture ?? string.Empty,
            VoteCount = voteCount,
            ElectionId = electionId
        };

        return electionInfo;
    }

    public List<CandidateOption>? GetAllForElection(Guid electionId)
    {
        string sqlQuery = $"SELECT * FROM candidates WHERE election_id = '{electionId}'";

        var dataReader = _db.ExecuteReadCommand(sqlQuery);

        List<CandidateOption> candidates = new List<CandidateOption>();

        while (dataReader.Read())
        {
            CandidateOption candidate = ReadCandidate(dataReader);
            Console.WriteLine(candidate.ToString());
            candidates.Add(candidate);
        }
        
        
        return candidates;
    } 


    public bool InsertElection(CandidateOption candidate, Guid electionId)
    {
        string sqlQuery = $"INSERT INTO candidates (id,title,address,picture,party,vote_count,election_id) VALUES " + 
                          $" ('{candidate.Id}','{candidate.Title}','{candidate.Address}','{candidate.Picture}','{candidate.Party}','{candidate.VoteCount}','{electionId}');";

        var affectedRows = _db.ExecuteWriteCommand(sqlQuery);
        
        if (affectedRows == 0)
        {
            return false;
        }
        
        return true;
    }

    public bool IncrementVote(Guid candidateId)
    {
        var affectedRows = _db.ExecuteIncrementProcedure(candidateId);

        if (affectedRows == 0)
        {
            return false;
        }

        return true;
    }
    
}
}