using EVT_FrontendApp.Services;
using EVT_FrontendApp.Views;
using System;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<MockAuthDataStore>();
            DependencyService.Register<MockElectionDataStore>();
            
            DependencyService.Register<AuthService>();
            DependencyService.Register<AlertService>();
            DependencyService.Register<VoteService>();
            
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
