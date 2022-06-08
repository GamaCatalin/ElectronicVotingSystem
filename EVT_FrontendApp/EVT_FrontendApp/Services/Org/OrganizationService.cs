using System;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.Services.Org
{
    public class OrganizationService : IOrganizationService
    {
        private IAccountService _accountService;

        public OrganizationService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Models.Organization> GetOrganizationAsync(string organizationId)
        {
            var organizationRepository = ViewModelLocator.Resolve<IRepository<Models.Organization>>();

            return await organizationRepository.Get(organizationId);
        }

        public async Task<Exception> SendInviteAsync(string email, string organizationId)
        {
          
            var invitedUser = await _accountService.GetUserByEmailAsync(email);

            if (invitedUser is null)
            {
                return new Exception("User not found!");
            }
            
            if (string.IsNullOrWhiteSpace(invitedUser.Email))
            {
                return new Exception("User does not exist!");
            }

            if (invitedUser.OrganizationId == organizationId)
            {
                return new Exception("User is already a member of this organization!");
            }
            

            if (!string.IsNullOrWhiteSpace(invitedUser.OrganizationId))
            {
                return new Exception("User already belongs to another organization!");
            }
            
            
            invitedUser.OrganizationId = organizationId;

            var updateTask = _accountService.UpdateUserAsync(invitedUser);

            if (updateTask.IsCanceled || updateTask.IsFaulted)
            {
                return new Exception("Could not invite user, please try again!");
            }

            await updateTask;

            await AddMemberToOrganizationAsync(invitedUser.Id, organizationId);

            return null;
        }

        public async Task<bool> AddMemberToOrganizationAsync(string userId, string organizationId)
        {
            var organizationRepository = RepositoryManager.GetOrganizationMembersRepository(organizationId);

            var orgMember = new OrganizationMember
            {
                Id = userId
            };

            var result = await organizationRepository.Save(orgMember);

            if (result != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveMemberFromOrganizationAsync(string userId, string organizationId)
        {
            var organizationMembersRepository = RepositoryManager.GetOrganizationMembersRepository(organizationId);
            var result = await organizationMembersRepository.Delete(userId);
            result = result && await _accountService.RemoveOrganizationFromUserAsync(userId);

            return result;
        }
        
        public async Task<bool> RemoveElectionFromOrganizationAsync(string electionId, string organizationId)
        {
            var organizationElectionsRepository = RepositoryManager.GetOrganizationElectionsRepository(organizationId);
            var result = await organizationElectionsRepository.Delete(electionId);

            var electionRepository = RepositoryManager.GetElectionRepository();
            result &= await electionRepository.Delete(electionId);

            return result;
        }

        public async Task<bool> DeleteOrganization(string organizationId)
        {
            var organizationMembersRepository = RepositoryManager.GetOrganizationMembersRepository(organizationId);
            var organizationElectionsRepository = RepositoryManager.GetOrganizationElectionsRepository(organizationId);
            var members = await organizationMembersRepository.GetAll();
            var elections = await organizationElectionsRepository.GetAll();
            
            foreach (var member in members)
            {
                if (!await RemoveMemberFromOrganizationAsync(member.Id, organizationId))
                {
                    return false;
                }
            }

            foreach (var election in elections)
            {
                if (!await RemoveElectionFromOrganizationAsync(election.Id, organizationId))
                {
                    return false;
                }
            }

            var result = await RepositoryManager.GetOrganizationRepository().Delete(organizationId);

            return result;
        }

        public async Task<bool> AddElectionToOrganizationAsync(string electionId, string organizationId)
        {
            var organizationRepository = RepositoryManager.GetOrganizationElectionsRepository(organizationId);

            var election = new PlainBallot
            {
                Id = electionId
            };

            var result = await organizationRepository.Save(election);

            if (result != electionId)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddOrganizationAsync(Organization organization)
        {
            var orgRepository = RepositoryManager.GetOrganizationRepository();

            var createdOrganizationId = await orgRepository.Save(organization);

            await AddMemberToOrganizationAsync(organization.Founder, createdOrganizationId);

            var result = await _accountService.SetUserOrganizationAsync(await _accountService.GetUserAsync(), createdOrganizationId);

            
            return true;
        }
    }
}