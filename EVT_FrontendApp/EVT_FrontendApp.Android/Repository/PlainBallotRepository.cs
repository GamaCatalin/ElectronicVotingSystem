using EVT_FrontendApp.Droid.Repository;
using EVT_FrontendApp.Models;
using Firebase.Auth;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlainBallotRepository))]
namespace EVT_FrontendApp.Droid.Repository
{
    public class PlainBallotRepository: BaseRepository<PlainBallot>
    {
        public PlainBallotRepository(string documentPath = "ballots") : base(documentPath)
        {
        }
        
        public PlainBallotRepository() : base("ballots")
        {
        }
    }
}