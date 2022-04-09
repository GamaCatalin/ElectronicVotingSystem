using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EVT_FrontendApp.Services;
using EVT_FrontendApp.Utils;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SignupCommand { get; }
        private string _username;
        private string _password;

        private readonly AuthService _authService;
        private AlertService _alertService;

        public LoginViewModel()
        {
            _authService = DependencyService.Get<AuthService>();
            _alertService = DependencyService.Get<AlertService>();
            LoginCommand = new Command(OnLoginClicked);
            SignupCommand = new Command(OnSignupClicked);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        private async void OnLoginClicked(object obj)
        {
            var loggedInSuccessfully = await _authService.LoginUser(_username, _password);


            if (loggedInSuccessfully)
            {
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            else
            {
                await _alertService.AlertAsync("Login failed!");
                // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");   
            }
        }

        private async void OnSignupClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignupPage)}");
            // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        } 
        
    }
}
