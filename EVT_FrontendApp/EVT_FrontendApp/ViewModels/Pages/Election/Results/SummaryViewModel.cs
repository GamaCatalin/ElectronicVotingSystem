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

namespace EVT_FrontendApp.ViewModels.Pages.Election.Results
{
    public class SummaryViewModel : BaseViewModel
    {
        private ElectionResults _selectedItem;
        public ElectionResults SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private ObservableCollection<ElectionResults> _items;
        public ObservableCollection<ElectionResults> Items
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

        public Command<ElectionResults> TapCommand { get; set; }

        private AuthenticatedUser _user;
        
        private INavigationService _navigatorService;
        private IElectionService _electionService;
        private readonly IAccountService _accountService;
        private readonly IAlertService _alertService;

        public SummaryViewModel(INavigationService navigatorService, IElectionService electionService,
            IAccountService accountService, IAlertService alertService)
        {
            _navigatorService = navigatorService;
            _electionService = electionService;
            _accountService = accountService;
            _alertService = alertService;

            Items = new ObservableCollection<ElectionResults>();
            LoadElectionsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            TapCommand = new Command<ElectionResults>((selectedItem) =>
            {
                _navigatorService.NavigateToAsync<ElectionResultsViewModel>(selectedItem);
            });
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                Items = await _electionService.GetAvailableElectionsResultsAsync(_user.Id);
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

        private void OnItemSelected(ElectionResults item)
        {
            if (item == null)
                return;

            _navigatorService.NavigateToAsync<ElectionResultsViewModel>(item);
        }
        public override async Task InitializeAsync(object navigationData = null)
        {
            _user = await _accountService.GetUserAsync();
            

            await ExecuteLoadItemsCommand();
            await base.InitializeAsync(navigationData);
        }
    }
}