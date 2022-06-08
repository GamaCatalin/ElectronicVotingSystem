using System.Threading.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrganizationMembersRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class OrganizationMembersRepository : BaseRepository<OrganizationMember>
    {
        public OrganizationMembersRepository(string documentPath = "") : base(documentPath)
        {
        }
        public OrganizationMembersRepository() : base("")
        {
        }
        
        public override Task<string> Save(OrganizationMember item)
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