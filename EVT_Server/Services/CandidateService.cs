using System.Collections;
using EVT_Server.Models.Election;
using EVT_Server.Repository;

namespace EVT_Server.Services{

public class CandidateService
{
    private CandidateRepository repo;

    public CandidateService()
    {
        repo = new CandidateRepository();
    }

      
    public List<CandidateOption> GetCandidatesForElection(Guid electionId)
    {
        var candidates = repo.GetAllForElection(electionId);
        return candidates ?? new List<CandidateOption>();
    }

    
    public List<CandidateOption> GetAll()
    {
        var candidates =  repo.GetAll();
        // Console.WriteLine(candidates.Count);
        return candidates;
    }
    

    
    
    
}
}