using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Org;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.Services.Election
{
    public class ElectionService : IElectionService
    {
        private IAccountService _accountService;
        private IOrganizationService _organizationService;

        public ElectionService(IAccountService accountService, IOrganizationService organizationService)
        {
            _accountService = accountService;
            _organizationService = organizationService;
        }

        public async Task<bool> VoteAsync(Ballot ballot, CandidateOption candidate, AuthenticatedUser user)
        {
            var voteRepository = RepositoryManager.GetVoteRepository(ballot.Id);
            var voteResult = false;

            if (await CheckIfUserVotedInElection(ballot.Id, user.Id))
            {
                return false;
            }

            try
            {
                await voteRepository.Save(new Vote()
                {
                    Id = user.Id,
                    CandidateId = candidate.Id
                });

                voteResult = true;
            }
            catch (Exception _)
            {
                // something went wrong
                voteResult = false;
            }

            return voteResult;
        }


        public async Task<ObservableCollection<Ballot>> GetVotedElectionsAsync(string userId)
        {
            var ballots = await GetOngoingElectionsAsync();

            var finBallots = new ObservableCollection<Ballot>();

            foreach (var ballot in ballots)
            {
                if (await CheckIfUserVotedInElection(ballot.Id, userId))
                {
                    finBallots.Add(ballot);
                }
            }

            return finBallots;
        }

        public async Task<ObservableCollection<Ballot>> GetValidElectionsAsync(string userId)
        {
            var ballots = await GetOngoingElectionsAsync();

            var finBallots = new ObservableCollection<Ballot>();

            foreach (var ballot in ballots)
            {
                if (!(await CheckIfUserVotedInElection(ballot.Id, userId)))
                {
                    finBallots.Add(ballot);
                }
            }

            return finBallots;
        }

        public async Task<ObservableCollection<ElectionResults>> GetAvailableElectionsResultsAsync(string userId)
        {
            var ballots = await GetFinishedElectionsAsync();

            var completedElections = new ObservableCollection<ElectionResults>();

            foreach (var election in ballots)
            {
                completedElections.Add(await GetElectionResults(election));
            }


            return completedElections;
        }


        private async Task<ElectionResults> GetElectionResults(Ballot election)
        {
            var electionVotes = await GetVotesForElectionAsync(election.Id);

            Dictionary<string, int> voteResults = new Dictionary<string, int>();
            var candidates = await GetCandidatesForElectionAsync(election.Id);

            foreach (var vote in electionVotes)
            {
                var candidateId = vote.CandidateId;
                if (!voteResults.ContainsKey(candidateId))
                {
                    voteResults[candidateId] = 0;
                }

                voteResults[candidateId]++;
            }


            string winnerId = null;
            if (voteResults.Count != 0)
            {
                winnerId = voteResults.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            }

            CandidateOption winner = null;
            foreach (var candidate in candidates)
            {
                if (voteResults.ContainsKey(candidate.Id))
                {
                    candidate.VoteCount = voteResults[candidate.Id];
                }
                else
                {
                    candidate.VoteCount = 0;
                }

                if (candidate.Id == winnerId)
                {
                    winner = candidate;
                }
            }

            candidates =
                new ObservableCollection<CandidateOption>(
                    candidates.OrderByDescending(candidate => candidate.VoteCount));

            return new ElectionResults
            {
                ElectionName = election.ElectionName,
                Results = voteResults,
                Winner = winner,
                Candidates = candidates
            };
        }


        public async Task<ObservableCollection<Ballot>> GetFinishedElectionsAsync()
        {
            var ballots = await GetAvailableElectionsAsync();

            var finBallots = new ObservableCollection<Ballot>();

            foreach (var ballot in ballots)
            {
                if (ballot.EndDate < DateTime.Now)
                {
                    finBallots.Add(ballot);
                }
            }

            return finBallots;
        }

        public async Task<ObservableCollection<Ballot>> GetOngoingElectionsAsync()
        {
            var ballots = await GetAvailableElectionsAsync();

            var finBallots = new ObservableCollection<Ballot>();

            foreach (var ballot in ballots)
            {
                if (ballot.EndDate > DateTime.Now && ballot.StartDate < DateTime.Now)
                {
                    finBallots.Add(ballot);
                }
            }

            return finBallots;
        }

        public async Task<ObservableCollection<Ballot>> GetAvailableElectionsAsync()
        {
            var ballotRepository = RepositoryManager.GetElectionRepository();
            var ballots = await ballotRepository.GetAll();
            var organizationId = await _accountService.GetUserOrganizationAsync();

            if (string.IsNullOrEmpty(organizationId))
            {
                // user is not in an organization
                return new ObservableCollection<Ballot>();
            }

            var finBallots = new ObservableCollection<Ballot>();

            foreach (var ballot in ballots)
            {
                if (ballot.OrganizationId == organizationId)
                {
                    finBallots.Add(ballot);
                }
            }

            return finBallots;
        }

        public async Task<CandidateOption> GetCandidateInElectionById(string electionId, string candidateId)
        {
            if (string.IsNullOrEmpty(candidateId) || string.IsNullOrEmpty(electionId))
            {
                return null;
            }

            var candidates = await GetCandidatesForElectionAsync(electionId);

            foreach (var candidate in candidates)
            {
                if (candidate.Id == candidateId)
                {
                    return candidate;
                }
            }

            return null;
        }

        public async Task<bool> CheckIfUserVotedInElection(string electionId, string userId)
        {
            var votes = await GetVotesForElectionAsync(electionId);

            return votes.Any(vote => vote.Id == userId);
        }

        public async Task<bool> AddElectionAsync(Ballot ballot)
        {
            var ballotRepository = RepositoryManager.GetBallotRepository();

            var electionId = await ballotRepository.Save((PlainBallot) ballot);


            await _organizationService.AddElectionToOrganizationAsync(electionId,
                (await _accountService.GetUserAsync()).OrganizationId);

            if (ballot.Options != null)
            {
                return await AddCandidatesForElectionAsync(electionId, ballot.Options);
            }

            return true;
        }

        public async Task<bool> AddCandidateForElectionAsync(string electionId, CandidateOption candidate)
        {
            var candidatesRepository = RepositoryManager.GetCandidateRepository(electionId);

            var result = await candidatesRepository.Add(candidate);

            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddCandidatesForElectionAsync(string electionId,
            IEnumerable<CandidateOption> candidates)
        {
            foreach (var candidate in candidates)
            {
                if (!await AddCandidateForElectionAsync(electionId, candidate))
                {
                    return false;
                }
            }

            return true;
        }


        public async Task<ObservableCollection<Vote>> GetVotesForElectionAsync(string electionId)
        {
            var voteRepository = RepositoryManager.GetVoteRepository(electionId);
            var votes = await voteRepository.GetAll();

            return new ObservableCollection<Vote>(votes);
        }

        public async Task<ObservableCollection<CandidateOption>> GetCandidatesForElectionAsync(string electionId)
        {
            var candidateRepository = RepositoryManager.GetCandidateRepository(electionId);
            var candidates = await candidateRepository.GetAll();


            return new ObservableCollection<CandidateOption>(candidates);
        }
    }
}