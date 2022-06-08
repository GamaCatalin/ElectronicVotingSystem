using System.Threading.Tasks;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Pages.Election.Ongoing;
using EVT_FrontendApp.ViewModels.Pages.Election.Results;
using EVT_FrontendApp.ViewModels.Pages.Org;
using EVT_FrontendApp.ViewModels.Pages.Profile;

namespace EVT_FrontendApp.ViewModels.Pages
{
    public class DashboardViewModel : BaseViewModel
    {

        private ProfileViewModel _profileViewModel;
        public ProfileViewModel ProfileViewModel
        {
            get => _profileViewModel;
            set => SetProperty(ref _profileViewModel, value);
        }


        private ElectionsViewModel _electionsViewModel;
        public ElectionsViewModel ElectionsViewModel
        {
            get => _electionsViewModel;
            set => SetProperty(ref _electionsViewModel, value);
        }


        private SummaryViewModel _summaryViewModel;
        public SummaryViewModel SummaryViewModel
        {
            get => _summaryViewModel;
            set => SetProperty(ref _summaryViewModel, value);
        }


        private OrganizationViewModel _organizationViewModel;
        public OrganizationViewModel OrganizationViewModel
        {
            get => _organizationViewModel;
            set => SetProperty(ref _organizationViewModel, value);
        }


        public DashboardViewModel(ProfileViewModel profileViewModel, ElectionsViewModel electionsViewModel, SummaryViewModel summaryViewModel, OrganizationViewModel organizationViewModel)
        {
            _profileViewModel = profileViewModel;
            _electionsViewModel = electionsViewModel;
            _summaryViewModel = summaryViewModel;
            _organizationViewModel = organizationViewModel;
        }

        public override Task InitializeAsync(object navigationData = null)
        {
            return Task.WhenAny(base.InitializeAsync(navigationData),
                OrganizationViewModel.InitializeAsync(navigationData),
                SummaryViewModel.InitializeAsync(null),
                ElectionsViewModel.InitializeAsync(null),
                ProfileViewModel.InitializeAsync(navigationData)
            );
        }
    }
}