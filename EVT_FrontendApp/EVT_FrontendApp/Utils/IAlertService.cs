using System.Threading.Tasks;

namespace EVT_FrontendApp.Utils
{
    public interface IAlertService
    {
        Task AlertAsync(string message);
        Task AlertAsync(string title, string message);
    }
}