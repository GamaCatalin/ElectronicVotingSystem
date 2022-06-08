using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Task = Android.Gms.Tasks.Task;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnCollectionCompleteListener<T>: Java.Lang.Object, IOnCompleteListener where T : IIdentifiable
    {

        private TaskCompletionSource<IList<T>> _tcs;

        public OnCollectionCompleteListener(TaskCompletionSource<IList<T>> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {

            if (task.IsSuccessful)
            {
                var docObjs = task.Result;
                if (docObjs is QuerySnapshot snapshot)
                {
                    _tcs.TrySetResult(snapshot.Convert<T>());
                }
            }
        }
    }
}