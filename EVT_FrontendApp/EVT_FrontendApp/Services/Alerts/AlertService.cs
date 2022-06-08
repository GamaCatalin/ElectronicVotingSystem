using System.Threading.Tasks;

namespace EVT_FrontendApp.Services.Alerts
{
    public class AlertService : IAlertService
    {
        public async Task AlertAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("Alert!", message, "Ok");
        }

        public async Task AlertAsync(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }

        public async Task<bool> OpenPrompt(string title, string message, string accept, string cancel)
        {
            var promptResult = await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            return promptResult;
        }
    }
}