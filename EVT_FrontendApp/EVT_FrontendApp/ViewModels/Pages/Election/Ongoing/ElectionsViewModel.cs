using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Election.Ongoing
{
    public class ElectionsViewModel : BaseViewModel
    {
        private Ballot _selectedItem;
        public Ballot SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private string _ballotElectionName;
        public string BallotElectionName
        {
            get => _ballotElectionName;
            set => SetProperty(ref _ballotElectionName, value);
        }

        private ObservableCollection<Ballot> _items;
        public ObservableCollection<Ballot> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }


        private ICommand _loadElectionsCommand;
        public ICommand LoadElectionsCommand
        {
            get => _loadElectionsCommand;
            set => SetProperty(ref _loadElectionsCommand, value);
        }

        public Command<Ballot> TapCommand { get; set; }

        private INavigationService _navigatorService;
        private IElectionService _electionService;
        private readonly IAccountService _accountService;
        private readonly IAlertService _alertService;

        public ElectionsViewModel(INavigationService navigatorService, IElectionService electionService,
            IAccountService accountService, IAlertService alertService)
        {
            _navigatorService = navigatorService;
            _electionService = electionService;
            _accountService = accountService;
            _alertService = alertService;

            Items = new ObservableCollection<Ballot>();
            LoadElectionsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            TapCommand = new Command<Ballot>((selectedItem) =>
            {
                _navigatorService.NavigateToAsync<BallotViewModel>(selectedItem);
            });
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                Items = await _electionService.GetValidElectionsAsync(await _accountService.GetUserIdAsync());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnItemSelected(Ballot item)
        {
            if (item == null)
                return;

            _navigatorService.NavigateToAsync<BallotViewModel>(item);
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            var user = await _accountService.GetUserAsync();
            

            await ExecuteLoadItemsCommand();
            await base.InitializeAsync(navigationData);
        }
    }
}