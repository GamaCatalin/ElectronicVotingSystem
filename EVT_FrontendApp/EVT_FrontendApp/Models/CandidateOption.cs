using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace EVT_FrontendApp.Models
{
    public class CandidateOption : IIdentifiable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }

        public string Picture { get; set; }
        public string Party { get; set; }
        public int VoteCount { get; set; } = 0;

        public static explicit operator ViewableCandidate(CandidateOption candidateOption) => new ViewableCandidate
        {
            Id = candidateOption.Id,
            Title = candidateOption.Title,
            Address = candidateOption.Address,
            Picture = candidateOption.Picture,
            Party = candidateOption.Party,
            VoteCount = candidateOption.VoteCount,
            BackgroundColor = Color.Peru,
            ProfilePicture = string.IsNullOrEmpty(candidateOption.Picture)
                ? ImageSource.FromFile("UserDefault_White_64")
                : ImageSource.FromUri(new Uri(candidateOption.Picture)),
        };
    }
}