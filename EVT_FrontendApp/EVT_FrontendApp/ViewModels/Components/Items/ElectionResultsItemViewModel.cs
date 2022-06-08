using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Pages.Election.Results;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Components.Items
{
    public class ElectionResultsItemViewModel : ExtendedBindableObject
    {
        private ElectionResults _electionResults;
        
        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        private string _electionName;
        public string ElectionName
        {
            get => _electionName;
            set => SetProperty(ref _electionName, value);
        }

        private string _electionWinner;

        public string ElectionWinner
        {
            get => _electionWinner;
            set => SetProperty(ref _electionWinner, value);
        }
        
        
        private ICommand _tapCommand;
        private readonly INavigationService _navigationService;

        public ICommand TapCommand
        {
            get => _tapCommand;
            set => SetProperty(ref _tapCommand, value);
        }


        public ElectionResultsItemViewModel(ElectionResults results, INavigationService navigationService)
        {
            _electionResults = results;
            _navigationService = navigationService;
            
            ElectionName = _electionResults.ElectionName;
            ElectionWinner = _electionResults.Winner.Title;


            TapCommand = OnItemSelected();
        }

        private ICommand OnItemSelected()
        {
            return new Command(NavigateToElectionResultsPage);
        }

        private async void NavigateToElectionResultsPage(object obj)
        {
            await _navigationService.NavigateToAsync<ElectionResultsViewModel>(_electionResults);
        }

    }
}