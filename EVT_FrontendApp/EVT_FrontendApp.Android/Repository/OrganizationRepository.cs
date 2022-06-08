using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrganizationRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class OrganizationRepository : BaseRepository<Organization>
    {
        public OrganizationRepository(string documentPath = "organizations") : base(documentPath)
        {
        }
        public OrganizationRepository() : base("organizations")
        {
        }
    }
}