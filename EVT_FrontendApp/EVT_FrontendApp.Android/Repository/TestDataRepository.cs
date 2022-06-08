using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Models;
using Xamarin.Forms;
using Firebase.Auth;
using Firebase.Firestore;

[assembly: Dependency(typeof(TestDataRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class TestDataRepository : BaseRepository<TestData>
    {
        public TestDataRepository()
        {
            SetDocumentPath(
                "users/" +
                FirebaseAuth.Instance.CurrentUser.Uid +
                "/test");
        }
    }
}