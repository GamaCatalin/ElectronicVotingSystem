using System.Threading.Tasks;
using Android.Gms.Tasks;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Task = Android.Gms.Tasks.Task;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnDeleteCompleteListener : Java.Lang.Object, IOnCompleteListener
    {

        private TaskCompletionSource _tcs;

        public OnDeleteCompleteListener(TaskCompletionSource tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {
            _tcs.TrySetResult(task.IsSuccessful);
        }
    }
}