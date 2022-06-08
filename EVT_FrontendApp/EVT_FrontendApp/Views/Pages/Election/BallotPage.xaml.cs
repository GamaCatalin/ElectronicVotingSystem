using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BallotPage : ContentPage
    {
        private readonly INavigationService _navigationService;

        public BallotPage()
        {
            InitializeComponent();
            _navigationService = ViewModelLocator.Resolve<INavigationService>();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        //
        // protected override bool OnBackButtonPressed()
        // {
        //     _navigationService.NavigateToAsync<DashboardViewModel>();
        //     // _navigationService.GoBackAsync();
        //     // return base.OnBackButtonPressed();
        //     return true;
        // }
    }
}