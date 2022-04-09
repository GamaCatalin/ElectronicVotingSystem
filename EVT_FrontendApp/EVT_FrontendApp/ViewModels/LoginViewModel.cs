using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SignupCommand { get; }
        private string username;
        private string password;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignupCommand = new Command(OnSignupClicked);
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        
        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Console.Out.WriteLineAsync(username + " --- " + password);
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnSignupClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignupPage)}");
            // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        } 
        
    }
}
