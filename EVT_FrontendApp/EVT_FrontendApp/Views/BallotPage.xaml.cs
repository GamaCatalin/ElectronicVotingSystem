using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVT_FrontendApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BallotPage : ContentPage
    {
        private BallotViewModel _viewModel;
        
        public BallotPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new BallotViewModel();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}