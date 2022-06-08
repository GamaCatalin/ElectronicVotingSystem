using System;
using System.Collections.Generic;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.Services.Image;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.Services.Org;
using EVT_FrontendApp.ViewModels.Pages;
using EVT_FrontendApp.ViewModels.Pages.Election.Ongoing;
using EVT_FrontendApp.ViewModels.Pages.Election.Results;
using EVT_FrontendApp.ViewModels.Pages.Login;
using EVT_FrontendApp.ViewModels.Pages.Org;
using EVT_FrontendApp.ViewModels.Pages.PasswordReset;
using EVT_FrontendApp.ViewModels.Pages.Profile;
using EVT_FrontendApp.ViewModels.Pages.Signup;
using EVT_FrontendApp.Views;
using EVT_FrontendApp.Views.Auth;
using EVT_FrontendApp.Views.Pages.Auth.Signup;
using EVT_FrontendApp.Views.Pages.Org;
using TinyIoC;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Base
{
    public class ViewModelLocator
    {
        private static TinyIoCContainer _container;

        private static Dictionary<Type, Type> _viewLookup;


        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();
            _viewLookup = new Dictionary<Type, Type>();
            
            // Register pages and page views
            Register<DashboardViewModel, DashboardPage>();
            Register<ProfileViewModel, ProfilePage>();
            Register<SummaryViewModel, SummaryPage>();
            
            Register<ElectionsViewModel, ElectionsPage>();
            Register<BallotViewModel, BallotPage>();
            Register<ElectionResultsViewModel, ElectionResultsPage>();

            Register<LoginViewModel, LoginPage>();
            // Register<LoginEmailViewModel, LoginEmailPage>();
            Register<LoginPhoneViewModel, LoginPhonePage>();
            Register<SignupViewModel, SignupPage>();
            Register<PasswordResetViewModel, PasswordResetPage>();
            
            
            Register<OrganizationViewModel, OrganizationPage>();
            Register<CreateOrganizationViewModel, CreateOrganizationPage>();
            Register<CreateElectionViewModel, CreateElectionPage>();
            
            

            // Register services (services are registered as Singletons default)
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IAccountService>(DependencyService.Get<IAccountService>());
            _container.Register<IImageService>(DependencyService.Get<IImageService>());
            _container.Register<IElectionService, ElectionService>();
            _container.Register<IAlertService, AlertService>();
            _container.Register<IOrganizationService, OrganizationService>();
            
            
            // Register repositories
            _container.Register(DependencyService.Get<IRepository<AuthenticatedUser>>());
            _container.Register(DependencyService.Get<IRepository<Ballot>>());
            _container.Register(DependencyService.Get<IRepository<PlainBallot>>());
            _container.Register(DependencyService.Get<IRepository<CandidateOption>>());
            _container.Register(DependencyService.Get<IRepository<Vote>>());
            _container.Register(DependencyService.Get<IRepository<Organization>>());
            _container.Register(DependencyService.Get<IRepository<OrganizationMember>>());

        }


        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }


        public static Page CreatePageFor(Type pageViewType)
        {
            var pageType = _viewLookup[pageViewType];
            var page = (Page) Activator.CreateInstance(pageType);
            var pageModel = _container.Resolve(pageViewType);

            page.BindingContext = pageModel;

            return page;
        }

        
        
        static void Register<TViewModel, TPage>() where TViewModel : BaseViewModel where TPage : Page
        {
            _viewLookup.Add(typeof(TViewModel), typeof(TPage));
            _container.Register<TViewModel>();
        }
        
    }
    
}