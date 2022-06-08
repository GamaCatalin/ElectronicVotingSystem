using System;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Services.Org
{
    public interface IOrganizationService
    {
        Task<Models.Organization> GetOrganizationAsync(string organizationId);

        Task<Exception> SendInviteAsync(string email, string organizationId);
        Task<bool> AddMemberToOrganizationAsync(string userId, string organizationId);
        Task<bool> RemoveMemberFromOrganizationAsync(string userId, string organizationId);

        Task<bool> DeleteOrganization(string organizationId);
        Task<bool> AddElectionToOrganizationAsync(string electionId, string organizationId);
        Task<bool> AddOrganizationAsync(Organization organization);
        
        
    }
}