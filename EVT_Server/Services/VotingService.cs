using EVT_Server.Repository;

namespace EVT_Server.Services;

public class VotingService
{
    private CandidateRepository repo;

    public VotingService()
    {
        repo = new CandidateRepository();
    }

    public async Task<bool> DoVote(Guid candidate)
    {
        return await repo.IncrementVote(candidate);
    }
}