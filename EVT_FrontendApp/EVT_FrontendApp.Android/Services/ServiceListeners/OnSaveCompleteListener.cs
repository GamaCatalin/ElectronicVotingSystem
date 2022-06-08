using System.Threading.Tasks;
using Android.Gms.Tasks;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<string>;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Task = Android.Gms.Tasks.Task;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnSaveCompleteListener : Java.Lang.Object, IOnCompleteListener
    {

        private TaskCompletionSource _tcs;

        public OnSaveCompleteListener(TaskCompletionSource tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {

            if (task.IsSuccessful)
            {
                if (task.Result is DocumentReference doc)
                {
                    _tcs.TrySetResult(doc.Id);
                }
            }

            _tcs.TrySetResult(default);
        }
    }
}