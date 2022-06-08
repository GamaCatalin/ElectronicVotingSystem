using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Services.Election
{
    public interface IElectionService
    {
        Task<bool> VoteAsync(Ballot ballot, CandidateOption candidate, AuthenticatedUser voter);
        Task<ObservableCollection<Ballot>> GetVotedElectionsAsync(string userId);
        Task<ObservableCollection<Ballot>> GetFinishedElectionsAsync();

        Task<ObservableCollection<Ballot>> GetAvailableElectionsAsync();
        Task<ObservableCollection<Ballot>> GetOngoingElectionsAsync();
        Task<ObservableCollection<Ballot>> GetValidElectionsAsync(string userId);

        Task<ObservableCollection<ElectionResults>> GetAvailableElectionsResultsAsync(string userId);

        Task<ObservableCollection<Vote>> GetVotesForElectionAsync(string electionId);
        Task<ObservableCollection<CandidateOption>> GetCandidatesForElectionAsync(string electionId);

        Task<CandidateOption> GetCandidateInElectionById(string electionId, string candidateId);
        Task<bool> CheckIfUserVotedInElection(string electionId, string userId);

        Task<bool> AddElectionAsync(Ballot ballot);

        Task<bool> AddCandidateForElectionAsync(string electionId, CandidateOption candidate);
        Task<bool> AddCandidatesForElectionAsync(string electionId, IEnumerable<CandidateOption> candidate);
    }
}