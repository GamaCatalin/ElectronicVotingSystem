using System.Threading.Tasks;
using Android.Gms.Tasks;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Task = Android.Gms.Tasks.Task;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnGetCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly TaskCompletionSource<AuthenticatedUser> _tcs;

        public OnGetCompleteListener(TaskCompletionSource<AuthenticatedUser> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                // process document

                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new AuthenticatedUser
                    {
                        Id = doc.Id,
                        FirstName = doc.GetString("FirstName"),
                        LastName = doc.GetString("LastName"),
                        OrganizationId = doc.GetString("OrganizationId"),
                        Email = doc.GetString("Email"),
                        ProfilePictureURL = doc.GetString("ProfilePictureURL")
                    };
                    _tcs.TrySetResult(user);
                }
            }

            // something went wrong  
            _tcs.TrySetResult(default(AuthenticatedUser));
        }
    }
}