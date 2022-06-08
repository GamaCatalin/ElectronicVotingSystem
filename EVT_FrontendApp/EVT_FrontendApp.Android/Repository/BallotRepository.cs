using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(BallotRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class BallotRepository: BaseRepository<Ballot>
    {
        public BallotRepository(string documentPath = "ballots") : base(documentPath)
        {
        }
        
        public BallotRepository() : base("ballots")
        {
        }
    }
}