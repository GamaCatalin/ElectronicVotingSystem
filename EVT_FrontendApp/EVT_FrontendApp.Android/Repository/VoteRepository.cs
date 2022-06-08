using System.Threading.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Firebase.Firestore;
using Xamarin.Forms;

[assembly: Dependency(typeof(VoteRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class VoteRepository: BaseRepository<Vote>
    {
        public VoteRepository(string documentPath = "") : base(documentPath)
        {
        }
        
        public VoteRepository() : base("")
        {
        }

        public override Task<string> Save(Vote item)
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