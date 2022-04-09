using EVT_FrontendApp.ViewModels;
using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Net;
using EVT_FrontendApp.Services;
using Xamarin.Forms;

namespace EVT_FrontendApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await DependencyService.Get<AuthService>().LogoutUser();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
