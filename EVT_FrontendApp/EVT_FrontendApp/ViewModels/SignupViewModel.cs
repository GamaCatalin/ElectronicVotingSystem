using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    public class SignupViewModel
    {
        public Command GoBackCommand { get; }

        
        public SignupViewModel()
        {
            GoBackCommand = new Command(OnBackPressed);
        }

        private async void OnBackPressed(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}