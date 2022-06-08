using System.Threading.Tasks;

namespace EVT_FrontendApp.Services.Alerts
{
    public interface IAlertService
    {
        Task AlertAsync(string message);
        Task AlertAsync(string title, string message);

        Task<bool> OpenPrompt(string title, string message, string accept = "Accept", string cancel = "Cancel");
    }
}