using System.Threading.Tasks;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;
using static EVT_FrontendApp.ViewModels.Base.BaseViewModel;

namespace EVT_FrontendApp.Services.Navigation
{
    public class NavigationService : INavigationService
    { 
        public async Task NavigateToAsync<TPageModelBase>(object navigationData = null, bool setRoot = false)
            where TPageModelBase : BaseViewModel
        {
            var page = ViewModelLocator.CreatePageFor(typeof(TPageModelBase));

            if (setRoot)
            {
                if (page is TabbedPage tabbedPage)
                {
                    App.Current.MainPage = new NavigationPage(tabbedPage);
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(page);
                }
            }
            else
            {
                if (page is TabbedPage tabbedPage)
                {
                    App.Current.MainPage = new NavigationPage(tabbedPage);
                }
                else if (App.Current.MainPage is NavigationPage navPage)
                {
                    await navPage.PushAsync(page);
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(page);
                }
            }

            if (page.BindingContext is BaseViewModel vmBase)
            {
                await vmBase.InitializeAsync(navigationData);
            }
        }

        public Task GoBackAsync()
        {
            return App.Current.MainPage.Navigation.PopAsync();
        }
    }
}