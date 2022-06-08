using System.Net.Http.Headers;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;

namespace EVT_FrontendApp.Repository
{
    public static class RepositoryManager
    {
        private static IRepository<Ballot> _electionRepository;
        private static IRepository<Organization> _organizationRepository;
        private static IRepository<PlainBallot> _ballotsRepository;


        public static IRepository<Ballot> GetElectionRepository()
        {
            if (_electionRepository == null)
            {
                _electionRepository = ViewModelLocator.Resolve<IRepository<Ballot>>();
            }

            return _electionRepository;
        }

        public static IRepository<Organization> GetOrganizationRepository()
        {
            if (_organizationRepository == null)
            {
                _organizationRepository = ViewModelLocator.Resolve<IRepository<Organization>>();
            }

            return _organizationRepository;
        }

        public static IRepository<CandidateOption> GetCandidateRepository(string electionId)
        {
            var candidateRepository = ViewModelLocator.Resolve<IRepository<CandidateOption>>();
            var documentPath =
                "ballots/" +
                electionId +
                "/candidates";
            candidateRepository.SetDocumentPath(documentPath);

            return candidateRepository;
        }

        public static IRepository<Vote> GetVoteRepository(string electionId)
        {
            var voteRepository = ViewModelLocator.Resolve<IRepository<Vote>>();
            var documentPath =
                "ballots/" +
                electionId +
                "/votes";

            voteRepository.SetDocumentPath(documentPath);

            return voteRepository;
        }

        public static IRepository<OrganizationMember> GetOrganizationMembersRepository(string organizationId)
        {
            var orgRepository = ViewModelLocator.Resolve<IRepository<OrganizationMember>>();
            var documentPath =
                "organizations/" +
                organizationId +
                "/members";

            orgRepository.SetDocumentPath(documentPath);

            return orgRepository;
        }

        public static IRepository<PlainBallot> GetOrganizationElectionsRepository(string organizationId)
        {
            var orgRepository = ViewModelLocator.Resolve<IRepository<PlainBallot>>();
            var documentPath =
                "organizations/" +
                organizationId +
                "/elections";

            orgRepository.SetDocumentPath(documentPath);

            return orgRepository;
        }

        public static IRepository<PlainBallot> GetBallotRepository()
        {
            if (_ballotsRepository is null)
            {
                _ballotsRepository = ViewModelLocator.Resolve<IRepository<PlainBallot>>();
            }

            return _ballotsRepository;
        }
    }
}