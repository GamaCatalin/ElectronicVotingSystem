using System;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Services.Account
{
    public interface IAccountService
    {

        Task<AggregateException> LoginAsync(string username, string password);
        Task<AggregateException> SignupAsync(string email, string password, AuthenticatedUser user);
        Task<AggregateException> SendOtpCodeAsync(string phoneNumber);
        Task<AggregateException> VerifyOtpCodeAsync(string code);

        Task<AuthenticatedUser> GetUserAsync();

        Task<string> UpdateUserAsync(AuthenticatedUser newUser);
        
        Task<string> GetUserOrganizationAsync();

        Task<string> GetUserIdAsync();

        Task<AuthenticatedUser> GetUserByEmailAsync(string email);

        Task<string> SetUserOrganizationAsync(AuthenticatedUser user, string organizationId);
        
        Task<bool> RemoveOrganizationFromUserAsync(string userId);

        void LogoutAsync();

        Task<bool> SendPasswordResetEmailAsync(string email);

    }
}
