using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace EVT_FrontendApp.Models
{
    public class ViewableCandidate : IIdentifiable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public string Party { get; set; }
        public int VoteCount { get; set; } = 0;
        public Color BackgroundColor { get; set; }
        public ImageSource ProfilePicture { get; set; }
        
        public static explicit operator CandidateOption(ViewableCandidate viewableCandidate) => new CandidateOption()
        {
            Id = viewableCandidate.Id,
            Title = viewableCandidate.Title,
            Address = viewableCandidate.Address,
            Picture = viewableCandidate.Picture,
            Party = viewableCandidate.Party,
            VoteCount = viewableCandidate.VoteCount,
        };
        
    }
}