using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Java.Lang;

namespace EVT_FrontendApp.Droid.Services.ServiceListeners
{
    public class OnUserFilterSnapshotListener: Java.Lang.Object, IEventListener
    {
        private TaskCompletionSource<AuthenticatedUser> _tcs;

        public OnUserFilterSnapshotListener(TaskCompletionSource<AuthenticatedUser> tcs)
        {
            _tcs = tcs;
        }

        public void OnEvent(Object obj, FirebaseFirestoreException error)
        {
            var query = (QuerySnapshot) obj;

            if (!query.IsEmpty)
            {
                var doc = query.Documents[0];
                _tcs.TrySetResult(new AuthenticatedUser
                {
                    Id = doc.Id,
                    FirstName = doc.GetString("FirstName"),
                    LastName = doc.GetString("LastName"),
                    OrganizationId = doc.GetString("OrganizationId"),
                    Email = doc.GetString("Email")
                });
            }

            _tcs.TrySetResult(default);
        }
    }
}