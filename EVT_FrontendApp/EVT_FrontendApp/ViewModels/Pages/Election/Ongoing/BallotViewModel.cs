using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;

namespace EVT_FrontendApp.ViewModels.Pages.Election.Ongoing
{
    public class BallotViewModel : BaseViewModel
    {
        private Ballot _selectedBallot;

        public Ballot SelectedBallot
        {
            get => _selectedBallot;
            set => SetProperty(ref _selectedBallot, value);
        }

        private ViewableCandidate _selectedItem;

        public ViewableCandidate SelectedItem
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

        
        public ButtonModel CastVoteModel { get; set; }


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
        
        public BallotViewModel(IAlertService alertService, IElectionService electionService, INavigationService navigationService, IAccountService accountService)
        {
            _alertService = alertService;
            _electionService = electionService;
            _navigationService = navigationService;
            _accountService = accountService;

            IsItemSelected = false;
            SelectedItem = null;
            
            Items = new ObservableCollection<ViewableCandidate>();

            CastVoteModel = new ButtonModel("CAST VOTE", OnCastVote);
        }


        private async void OnCastVote()
        {
            if(!ValidateItemSelected())
            {
                await _alertService.AlertAsync("No candidate has been selected");
                return;
            }
            
            var promptResult = await _alertService.OpenPrompt("Vote confirmation", "Confirm vote?");


            if (promptResult)
            {
                // continue with vote
                
                var currentUser = await _accountService.GetUserAsync();
                // SelectedItem = 
                var voteResult = await _electionService.VoteAsync(SelectedBallot, (CandidateOption)SelectedItem, currentUser);
                
                if (voteResult)
                {
                    // vote passed
                    await _alertService.AlertAsync("Vote result", "Vote passed");
                    
                }
                else
                {
                    // vote failed
                    await _alertService.AlertAsync("Vote failed!");
                }

                // await _navigationService.NavigateToAsync<ElectionsViewModel>();
                await _navigationService.NavigateToAsync<DashboardViewModel>();
            }
            else
            {
                // vote canceled
            }
            
        }

        private bool ValidateItemSelected()
        {
            return SelectedItem != null;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (_selectedBallot != null)
            {
                Items = FromCandidates(await _electionService.GetCandidatesForElectionAsync(_selectedBallot.Id));
            }
            else
            {
                Items = new ObservableCollection<ViewableCandidate>();
            }
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
        
        private Task ExecuteLoadTitleCommand()
        {
            Title = SelectedBallot != null ? _selectedBallot.ElectionName : "Null election";
            return Task.CompletedTask;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            SelectedBallot = (Ballot) navigationData;

            await ExecuteLoadTitleCommand();

            await ExecuteLoadItemsCommand();

            SelectedItem = null;

            IsItemSelected = false;

            await base.InitializeAsync(navigationData);
        }
    }
}