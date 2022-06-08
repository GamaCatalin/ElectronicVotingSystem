using System;
using System.Threading.Tasks;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;
using EVT_FrontendApp.ViewModels.Pages.PasswordReset;
using EVT_FrontendApp.ViewModels.Pages.Signup;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        
        
        public TextEntryViewModel EmailEntryViewModel { get; set; }
        public TextEntryViewModel PasswordEntryViewModel { get; set; }


        public ButtonModel ForgotPasswordModel { get; set; }
        public ButtonModel LogInModel { get; set; }
        public ButtonModel SignUpModel { get; set; }
        public ButtonModel UsePhoneModel { get; set; }
        

        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IAlertService _alertService;
        
        public LoginViewModel(INavigationService navigationService , IAccountService accountService, IAlertService alertService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _alertService = alertService;
            
            OSAppTheme appTheme = App.Current.UserAppTheme;
            Color bgColor = appTheme == OSAppTheme.Dark
                ? (Color)App.Current.Resources["SecondaryBackgroundColorDark"]
                : (Color)App.Current.Resources["SecondaryBackgroundColor"];

            EmailEntryViewModel = new TextEntryViewModel("Email");
            PasswordEntryViewModel = new TextEntryViewModel("Password", true);



            ForgotPasswordModel = new ButtonModel("Forgot Password?", OnForgotPassword);
            LogInModel = new ButtonModel("LOG IN", OnLogin);
            SignUpModel = new ButtonModel("SIGN UP", GoToSignup);
            UsePhoneModel = new ButtonModel("USE PHONE NUMBER", GoToPhoneLogin);

        }
        
        
        private void GoToSignup()
        {
            _navigationService.NavigateToAsync<SignupViewModel>();
        }

        private async void OnLogin()
        {            
            var email = EmailEntryViewModel.Text;
            var password = PasswordEntryViewModel.Text;

            if (!ValidateEmail())
            {
                return;
            }

            if (!ValidatePassword())
            {
                return;
            }

            IsBusy = true;
            
            var loginException = await _accountService.LoginAsync(email, password);
            if (loginException is null)
            {
                var connectedUser = await _accountService.GetUserAsync();
                await _navigationService.NavigateToAsync<DashboardViewModel>(connectedUser, true);                
            }
            else
            {
                HandleLoginException(loginException);
                // await _alertService.AlertAsync("Invalid credentials!");
            }

            IsBusy = false;
        }

        private bool ValidateEmail()
        {
            var email = EmailEntryViewModel.Text;
            EmailEntryViewModel.FieldError = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                EmailEntryViewModel.FieldError = "Email is empty!";
                return false;
            }
            
            return true;
        }
        
        private bool ValidatePassword()
        {
            var password = PasswordEntryViewModel.Text;
            PasswordEntryViewModel.FieldError = "";
            
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordEntryViewModel.FieldError = "Password is empty!";
                return false;
            }

            return true;
        }

        private async void HandleLoginException(AggregateException loginException)
        {
            if (loginException.InnerExceptions.Count > 0)
            {
                var innerExceptionMessage = loginException.InnerExceptions[0].Message;
                if (innerExceptionMessage == "The email address is badly formatted.")
                {
                    EmailEntryViewModel.FieldError = "Invalid email address!";
                }
                else if (innerExceptionMessage == "The password is invalid or the user does not have a password.")
                {
                    PasswordEntryViewModel.FieldError = "Invalid password!";
                }
                else if (innerExceptionMessage == "There is no user record corresponding to this identifier. The user may have been deleted.")
                {
                    EmailEntryViewModel.FieldError = "User does not exist!";
                }
                else if (innerExceptionMessage == "Email is not verified!")
                {
                    await _alertService.AlertAsync("Login error", "Email not verified, please check inbox!");
                }
                else
                {
                    await _alertService.AlertAsync("Login exception", innerExceptionMessage);
                }   
            }
            else
            {
                var loginExceptionMessage = loginException.Message;
                
                if (loginExceptionMessage == "Email is not verified!")
                {
                    await _alertService.AlertAsync("Login error", "Email not verified, please check your inbox!");
                }
                else
                {
                    await _alertService.AlertAsync("Login exception", loginExceptionMessage);
                }   
            }


            
            
        }
        private void OnForgotPassword()
        {
            _navigationService.NavigateToAsync<PasswordResetViewModel>();
        }


        private void GoToPhoneLogin()
        {
            _navigationService.NavigateToAsync<LoginPhoneViewModel>();
        }

        public override Task InitializeAsync(object navigationData = null)
        {
            
            return base.InitializeAsync(navigationData);
        }
    }
}
