using System;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Items
{
    public class CandidateItemViewModel : ExtendedBindableObject
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


        private string _party;
        public string Party
        {
            get => _party;
            set => SetProperty(ref _party, value);
        }


        public CandidateItemViewModel(ViewableCandidate candidate)
        {
            _candidate = candidate;

            Title = _candidate.Title;
            Party = _candidate.Party;
        }
    }
}