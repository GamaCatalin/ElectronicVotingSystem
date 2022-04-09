using System.Linq;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using Xamarin.Forms;

namespace EVT_FrontendApp.Services
{
    public class VoteService
    {
        private IDataStore<Ballot> _voteData;
        
        
        
        public VoteService()
        {
            _voteData = DependencyService.Get<MockElectionDataStore>();
        }

        public async Task<bool> CastVote(string ballotId, string candidateId)
        {
            var ballots = await _voteData.GetItemsAsync(true);
            var votedBallot = ballots.FirstOrDefault(ballot => ballot.Id == ballotId);

            if (votedBallot == default(Ballot))
            {
                return false;
            }

            var votedCandidate = votedBallot.Options.FirstOrDefault(candidate => candidate.Id == candidateId);

            if (votedCandidate == default(CandidateOption))
            {
                return false;
            }

            votedBallot.HasVoted = true; // block access to a certain ballot
            votedCandidate.VoteCount++; // increase the vote count

            return true;
        }
    }
}