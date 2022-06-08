using System.Threading.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Firebase.Firestore;
using Xamarin.Forms;

[assembly: Dependency(typeof(CandidateRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class CandidateRepository : BaseRepository<CandidateOption>
    {
        public CandidateRepository(string documentPath = "") : base(documentPath)
        {
        }
        public CandidateRepository() : base("")
        {
        }
        
        public override Task<string> Save(CandidateOption item)
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