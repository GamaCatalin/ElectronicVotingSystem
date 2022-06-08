using System.Threading.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Firebase.Firestore;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class UserRepository : BaseRepository<AuthenticatedUser>
    {
        public UserRepository(string documentPath = "users") : base(documentPath)
        {
        }
        public UserRepository() : base("users")
        {
        }
        
        public override Task<string> Save(AuthenticatedUser item)
        {
            var tcs = new TaskCompletionSource<string>();

            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Document(item.Id)
                .Set(item.Convert())
                .AddOnCompleteListener(new OnSaveCompleteListener(tcs));

            return tcs.Task;
        }
    }
}