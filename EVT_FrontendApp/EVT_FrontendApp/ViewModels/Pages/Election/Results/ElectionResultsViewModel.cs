using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.ViewModels.Pages.Election.Results
{
    public class ElectionResultsViewModel : BaseViewModel
    {
        private ElectionResults _selectedElectionResults;

        public ElectionResults SelectedElectionResults
        {
            get => _selectedElectionResults;
            set => SetProperty(ref _selectedElectionResults, value);
        }

        private CandidateOption _selectedItem;

        public CandidateOption SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private ObservableCollection<ViewableCandidate> _items;

        public ObservableCollection<ViewableCandidate> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        

        private bool _isItemSelected;
        public bool IsItemSelected
        {
            get => _isItemSelected;
            set => SetProperty(ref _isItemSelected, value);
        }


        private IAlertService _alertService;
        private IElectionService _electionService;
        private INavigationService _navigationService;
        private IAccountService _accountService;
        
        public ElectionResultsViewModel(IAlertService alertService, IElectionService electionService, INavigationService navigationService, IAccountService accountService)
        {
            _alertService = alertService;
            _electionService = electionService;
            _navigationService = navigationService;
            _accountService = accountService;

            IsItemSelected = false;
            SelectedItem = null;
            
            Items = new ObservableCollection<ViewableCandidate>();

        }


        private void ExecuteLoadItemsCommand()
        {
            if (_selectedElectionResults != null)
            {
                Items = FromCandidates(_selectedElectionResults.Candidates);
            }
            else
            {
                Items = new ObservableCollection<ViewableCandidate>();
            }
        }

        private void ExecuteLoadTitleCommand()
        {
            Title = SelectedElectionResults != null ? _selectedElectionResults.ElectionName : "Null election";
        }

        private ObservableCollection<ViewableCandidate> FromCandidates(ObservableCollection<CandidateOption> candidates)
        {
            var viewableCandidates = new ObservableCollection<ViewableCandidate>();

            foreach (var candidate in candidates)
            {
                viewableCandidates.Add((ViewableCandidate) candidate);
            }

            return viewableCandidates;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            SelectedElectionResults = (ElectionResults) navigationData;

            ExecuteLoadTitleCommand();
            
            ExecuteLoadItemsCommand();

            SelectedItem = null;

            IsItemSelected = false;
            
            await base.InitializeAsync(navigationData);
        }
    }
}