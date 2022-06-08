using EVT_FrontendApp.Services;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Views;
using System;
using System.Threading.Tasks;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Pages.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();            
        }

        Task InitNavigation()
        {
            var navService = ViewModelLocator.Resolve<INavigationService>();
            return navService.NavigateToAsync<LoginViewModel>(null, true);
        }

        protected override async void OnStart()
        {
            await InitNavigation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
