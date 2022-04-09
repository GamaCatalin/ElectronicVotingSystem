using System.Threading.Tasks;
using Xamarin.Forms;

namespace EVT_FrontendApp.Utils
{
    public class AlertService : IAlertService
    {
        public async Task AlertAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("ALERT!", message, "OK");
        }
    }
}