using EVT_Server.Repository;

namespace EVT_Server.Services
{

    public class VotingService
    {
        private CandidateRepository repo;

        public VotingService()
        {
            repo = new CandidateRepository();
        }

        public bool DoVote(Guid candidate)
        {
            return repo.IncrementVote(candidate);
        }
    }
}