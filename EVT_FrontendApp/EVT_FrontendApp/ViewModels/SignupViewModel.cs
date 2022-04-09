using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using EVT_FrontendApp.Services;
using EVT_FrontendApp.Utils;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }

        private string _username;
        private string _password;

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

        public SignupViewModel()
        {
            RegisterCommand = new Command(OnRegisterPressed);
        }

        private async void OnRegisterPressed(object obj)
        {
            var registeredSuccessfully = await AuthService.RegisterUser(_username, _password);


            if (registeredSuccessfully)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await AlertService.AlertAsync("Register failed!");
            }
        }
    }
}