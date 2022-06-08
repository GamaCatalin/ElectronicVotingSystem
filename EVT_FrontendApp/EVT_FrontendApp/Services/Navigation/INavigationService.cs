using System.Threading.Tasks;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation method to push onto the Navigation Stack
        /// </summary>
        /// <param name="navigationData"></param>
        /// <param name="setRoot"></param>
        /// <typeparam name="TPageModelBase"></typeparam>
        /// <returns></returns>
        Task NavigateToAsync<TPageModelBase>(object navigationData = null, bool setRoot = false)
            where TPageModelBase : BaseViewModel;
        
        /// <summary>
        /// Navigation method to pop off the Navigation Stack
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();
    }
}