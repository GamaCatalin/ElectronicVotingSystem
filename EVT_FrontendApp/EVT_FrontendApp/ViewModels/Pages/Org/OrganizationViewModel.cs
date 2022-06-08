using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.Services.Org;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Org
{
    public class OrganizationViewModel : BaseViewModel
    {
        private static AuthenticatedUser _user;

        public AuthenticatedUser User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private Organization _organization;

        public Organization Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        private bool _isMemberOfAnOrganization;

        public bool IsMemberOfAnOrganization
        {
            get => _isMemberOfAnOrganization;
            set => SetProperty(ref _isMemberOfAnOrganization, value);
        }

        private bool _isFounderOfOrganization;

        public bool IsFounderOfOrganization
        {
            get => _isFounderOfOrganization;
            set => SetProperty(ref _isFounderOfOrganization, value);
        }

        private string _organizationName;

        public string OrganizationName
        {
            get => _organizationName;
            set => SetProperty(ref _organizationName, value);
        }

        private string _organizationRank;

        public string OrganizationRank
        {
            get => _organizationRank;
            set => SetProperty(ref _organizationRank, value);
        }


        private ICommand _loadOrganizationCommand;

        public ICommand LoadOrganizationCommand
        {
            get => _loadOrganizationCommand;
            set => SetProperty(ref _loadOrganizationCommand, value);
        }

        public TextEntryViewModel ReferralEntryViewModel { get; set; }
        public ButtonModel ReferralButtonModel { get; set; }
        public ButtonModel CreateOrganizationButtonModel { get; set; }
        public ButtonModel CreateElectionButtonModel { get; set; }
        public ButtonModel LeaveOrganizationButtonModel { get; set; }
        public ButtonModel DeleteOrganizationButtonModel { get; set; }


        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IOrganizationService _organizationService;
        private IAlertService _alertService;

        public OrganizationViewModel(INavigationService navigationService, IAccountService accountService,
            IOrganizationService organizationService, IAlertService alertService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _organizationService = organizationService;
            _alertService = alertService;

            OrganizationName = "";


            ReferralEntryViewModel = new TextEntryViewModel("Email");
            ReferralButtonModel = new ButtonModel("INVITE", OnInviteSend);
            CreateOrganizationButtonModel = new ButtonModel("CREATE ORGANIZATION", OnCreateOrganization);
            CreateElectionButtonModel = new ButtonModel("CREATE NEW ELECTION", OnCreateElection);
            LeaveOrganizationButtonModel = new ButtonModel("Leave organization", OnLeaveOrganization);
            DeleteOrganizationButtonModel = new ButtonModel("Delete organization", OnDeleteOrganization);



            LoadOrganizationCommand = new Command(LoadOrganization);
        }

        private async void OnDeleteOrganization()
        {
            var userId = _user.Id;
            var organizationId = _organization.Id;


            var promptResult =
                await _alertService.OpenPrompt("Delete warning", "Are you sure you want to delete the organization?");

            if (!promptResult)
            {
                return;
            }
            

            IsBusy = true;

            var result = await _organizationService.DeleteOrganization(organizationId);

            if (result)
            {
                LoadOrganization();
            }
            else
            {
                await _alertService.AlertAsync("Could not delete organization, something went wrong!");
            }
            
            IsBusy = false;
        }

        private async void OnLeaveOrganization()
        {
            var userId = _user.Id;
            var organizationId = _organization.Id;

            var promptResult =
                await _alertService.OpenPrompt("Leave warning", "Are you sure you want to leave the organization?");

            if (!promptResult)
            {
                return;
            }
            
            
            IsBusy = true;

            var result = await _organizationService.RemoveMemberFromOrganizationAsync(userId, organizationId);

            if (result)
            {
                LoadOrganization();
            }
            else
            {
                await _alertService.AlertAsync("Could not leave organization, something went wrong!");
            }
            
            IsBusy = false;
        }

        private async void OnCreateElection()
        {
            await _navigationService.NavigateToAsync<CreateElectionViewModel>();
        }

        private async void OnCreateOrganization()
        {
            await _navigationService.NavigateToAsync<CreateOrganizationViewModel>();
            var vm = ViewModelLocator.Resolve<CreateOrganizationViewModel>();
            vm.PropertyChanged += CheckForIsBusy;
            
        }

        private void CheckForIsBusy(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
            {
                var vm = (CreateOrganizationViewModel) sender;
                if (vm.IsBusy == false)
                {
                    LoadOrganization();                    
                }
            }
        }


        public override Task InitializeAsync(object navigationData = null)
        {
            if (navigationData is AuthenticatedUser user)
            {
                User = user;
            }

            LoadOrganization();
            if (IsMemberOfAnOrganization)
            {
                LoadReferralComponents();
            }


            return base.InitializeAsync(navigationData);
        }

        private void LoadReferralComponents()
        {
            ReferralEntryViewModel = new TextEntryViewModel("Email");
            ReferralButtonModel = new ButtonModel("INVITE", OnInviteSend);
        }

        private async void OnInviteSend()
        {
            var email = ReferralEntryViewModel.Text;

            if (ValidateEmail(email))
            {
                var inviteResult = await _organizationService.SendInviteAsync(email, Organization.Id);

                if (inviteResult is null)
                {
                    // user invited successfully
                    await _alertService.AlertAsync("User was added to " + Organization.OrganizationName + "!");
                    ReferralEntryViewModel.Text = "";
                }
                else
                {
                    await _alertService.AlertAsync(inviteResult.Message);
                }
            }
        }


        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ReferralEntryViewModel.FieldError = "Email must not be empty!";

                return false;
            }

            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (!emailRegex.IsMatch(email))
            {
                ReferralEntryViewModel.FieldError = "Email is not valid!";

                return false;
            }

            return true;
        }


        private async void LoadOrganization()
        {
            IsBusy = true;
            IsFounderOfOrganization = false;
            User = await _accountService.GetUserAsync();
            var organizationId = User.OrganizationId;

            if (string.IsNullOrWhiteSpace(organizationId))
            {
                // OrganizationStatus = "Currently not a member of any organization!";
                IsMemberOfAnOrganization = false;
            }
            else
            {
                Organization = await _organizationService.GetOrganizationAsync(organizationId);

                if (Organization.Founder == User.Id)
                {
                    OrganizationRank = "Founder";
                    IsFounderOfOrganization = true;
                }
                else
                {
                    OrganizationRank = "Member";
                }


                OrganizationName = Organization.OrganizationName;
                IsMemberOfAnOrganization = true;
            }

            IsBusy = false;
        }
    }
}