using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.Services.Org;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;

namespace EVT_FrontendApp.ViewModels.Pages.Org
{
    public class CreateOrganizationViewModel : BaseViewModel
    {
        public TextEntryViewModel OrganizationNameEntryViewModel { get; set; }

        public ButtonModel CreateOrganizationModel { get; set; }
        
        public bool IsInUse { get; set; }


        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IAlertService _alertService;
        private IOrganizationService _organizationService;


        public CreateOrganizationViewModel(IAlertService alertService, IAccountService accountService,
            INavigationService navigationService, IOrganizationService organizationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _alertService = alertService;
            _organizationService = organizationService;
           
           
            OrganizationNameEntryViewModel = new TextEntryViewModel("Organization Name");            
            CreateOrganizationModel = new ButtonModel("CREATE ORGANIZATION", OnCreateOrganizationAction);

            IsInUse = true;
        }

        private async void OnCreateOrganizationAction()
        {
            if (!ValidateOrganizationName())
            {
                IsBusy = false;
                IsInUse = false;
                return;
            }

            IsBusy = true;
        
            
            var organization = await CreateOrganization();

            var addOrganizationException = await _organizationService.AddOrganizationAsync(organization);

            if (addOrganizationException)
            {
                // await _navigationService.GoBackAsync();
                await _navigationService.NavigateToAsync<DashboardViewModel>();
            }
            else
            {
                await _alertService.AlertAsync("Organization creation failed!");
            }

            IsBusy = false;
            IsInUse = false;
        }

        private async Task<Organization> CreateOrganization()
        {
            var orgName = OrganizationNameEntryViewModel.Text;
            var founder = await _accountService.GetUserAsync();
            
            return new Organization
            {
                OrganizationName = orgName,
                Founder = founder.Id
            };
        }
        
        private bool ValidateOrganizationName()
        {
            var orgName = OrganizationNameEntryViewModel.Text;

            if (string.IsNullOrWhiteSpace(orgName))
            {
                return false;
            }

            return true;
        }
    }
    
    
}