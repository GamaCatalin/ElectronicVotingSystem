using System.Threading.Tasks;
using Android.Gms.Tasks;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Task = Android.Gms.Tasks.Task;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnResetPasswordCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly TaskCompletionSource<bool> _tcs;

        public OnResetPasswordCompleteListener(TaskCompletionSource<bool> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                _tcs.TrySetResult(true);
            }

            // something went wrong  
            _tcs.TrySetResult(false);
        }
    }
}