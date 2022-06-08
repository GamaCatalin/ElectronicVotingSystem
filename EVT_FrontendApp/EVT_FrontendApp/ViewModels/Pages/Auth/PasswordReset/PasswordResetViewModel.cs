using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;

namespace EVT_FrontendApp.ViewModels.Pages.PasswordReset
{
    public class PasswordResetViewModel : BaseViewModel
    {
        public TextEntryViewModel EmailEntryViewModel { get; set; }
        public ButtonModel ResetPasswordModel { get; set; }


        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IAlertService _alertService;
        
        

        public PasswordResetViewModel(IAlertService alertService, IAccountService accountService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _alertService = alertService;

            
            EmailEntryViewModel = new TextEntryViewModel("Email");
            
            ResetPasswordModel = new ButtonModel("RESET PASSWORD", OnResetPasswordAction);
        }

        private async void OnResetPasswordAction()
        {
            if (!ValidateEmail())
            {
                return;
            }
           
            IsBusy = true;
        
            var email = EmailEntryViewModel.Text;
            
            var passwordResetStatus = await _accountService.SendPasswordResetEmailAsync(email);

            if (passwordResetStatus)
            {
                // await _navigationService.NavigateToAsync<LoginViewModel>();
                IsBusy = false;
                await _alertService.AlertAsync("Reminder", "Password reset link sent successfully, remember to also check spam!");
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _alertService.AlertAsync("There was an error while processing the password reset request!");
            }

            IsBusy = false;
        }

        private bool ValidateEmail()
        {
            var email = EmailEntryViewModel.Text;
            EmailEntryViewModel.FieldError = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                EmailEntryViewModel.FieldError = "Email must not be empty!";
                    
                return false;
            }

            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (!emailRegex.IsMatch(email))
            {
                EmailEntryViewModel.FieldError = "Email is not valid!";

                return false;
            }
                        
            return true;
        }
    }
    
    
}