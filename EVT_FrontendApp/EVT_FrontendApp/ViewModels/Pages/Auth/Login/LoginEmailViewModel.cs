using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.ViewModels.Pages.Login
{
    public class LoginEmailViewModel : BaseViewModel
    {
        // private ICommand _loginCommand;
        //
        // public ICommand LoginCommand
        // {
        //     get => _loginCommand;
        //     set => SetProperty(ref _loginCommand, value);
        // }
        //
        // private ICommand _signupCommand;
        // public ICommand SignupCommand
        // {
        //     get => _signupCommand;
        //     set => SetProperty(ref _signupCommand, value);
        // }
        //
        // private string _username;
        // private string _password;
        //
        //
        // private INavigationService _navigationService;
        // private IAccountService _accountService;
        // private IAlertService _alertService;
        //
        // public LoginEmailViewModel(INavigationService navigationService , IAccountService accountService, IAlertService alertService)
        // {
        //     _navigationService = navigationService;
        //     _accountService = accountService;
        //     _alertService = alertService;
        //
        //     Username = "";
        //     Password = "";
        //     
        //     LoginCommand = new Command(OnLoginAction);
        //     SignupCommand = new Command(OnSignupAction);
        // }
        //
        // public string Username
        // {
        //     get => _username;
        //     set => SetProperty(ref _username, value);
        // }
        //
        // public string Password
        // {
        //     get => _password;
        //     set => SetProperty(ref _password, value);
        // }
        //
        // private async void OnLoginAction(object obj)
        // {
        //
        //     var isNullOrEmpty = Username.Equals("") || Password.Equals("");
        //     var loginAttempt =  !isNullOrEmpty && await _accountService.LoginAsync(Username, Password);
        //     if (loginAttempt)
        //     {
        //         await _navigationService.NavigateToAsync<DashboardViewModel>(null, true);                
        //     }
        //     else
        //     {
        //         await _alertService.AlertAsync("Invalid credentials!");
        //     }
        //     
        //
        // }
        //
        // private async void OnSignupAction(object obj)
        // {
        //     await _navigationService.NavigateToAsync<SignupViewModel>();
        // } 

    }
}