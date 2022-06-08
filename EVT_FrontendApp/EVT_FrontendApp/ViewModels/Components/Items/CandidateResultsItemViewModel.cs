using System;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace EVT_FrontendApp.ViewModels.Items
{
    public class CandidateResultsItemViewModel : ExtendedBindableObject
    {
        private ViewableCandidate _candidate;

        public ViewableCandidate Candidate
        {
            get => _candidate;
        }

        public Color BackgroundColor = Color.Red;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get => _profilePicture;
            set => SetProperty(ref _profilePicture, value);
        }

        private string _party;
        public string Party
        {
            get => _party;
            set => SetProperty(ref _party, value);
        }

        private string _voteCount;

        public string VoteCount
        {
            get => _voteCount;
            set => SetProperty(ref _voteCount, value);
        }


        public CandidateResultsItemViewModel(ViewableCandidate candidate)
        {
            _candidate = candidate;

            Title = _candidate.Title;
            Party = _candidate.Party;

            ProfilePicture = string.IsNullOrEmpty(_candidate.Picture)
                ? ImageSource.FromFile("UserDefault_64")
                : ImageSource.FromUri(new Uri(_candidate.Picture));
        }
    }
}