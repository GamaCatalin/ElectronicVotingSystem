using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Entries;
using EVT_FrontendApp.ViewModels.Pages.Org;
using EVT_FrontendApp.Views.Entries;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Items
{
    public class CandidateCreatorItemViewModel : ExtendedBindableObject
    {
        private ViewableCandidate _candidate;

        public ViewableCandidate Candidate
        {
            get => _candidate;
        }

        private string _itemIndex;

        public string ItemIndex
        {
            get => _itemIndex;
            set => SetProperty(ref _itemIndex, value);
        }
        
        public TextEntryViewModel TitleEntry { get; set; }
        public TextEntryViewModel PartyEntry { get; set; }

        private CreateElectionViewModel _issuedPage;
        
       
        private string _party;
        public string Party
        {
            get => _party;
            set => SetProperty(ref _party, value);
            
            
        }


        public CandidateCreatorItemViewModel(CreateElectionViewModel issuedPage)
        {
            _issuedPage = issuedPage;

            TitleEntry = new TextEntryViewModel("Title");
            PartyEntry = new TextEntryViewModel("Party (optional)");

            TitleEntry.PropertyChanged += PropertyChangedTitle;
            PartyEntry.PropertyChanged += PropertyChangedParty;
        }

        private void PropertyChangedParty(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedTitle(sender, e);
        }

        private void PropertyChangedTitle(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                
                var candidateEntries = _issuedPage.CandidateEntries;
                
                var currentElemIndex = candidateEntries.IndexOf(this);

                if (currentElemIndex >= 0)
                {
                    TitleEntry.FieldError = "";
                    
                    if(string.IsNullOrWhiteSpace(TitleEntry.Text) && string.IsNullOrWhiteSpace(PartyEntry.Text) && candidateEntries.Count>1)
                    {
                        _issuedPage.CandidateEntries.RemoveAt(currentElemIndex);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(TitleEntry.Text) && !string.IsNullOrWhiteSpace(PartyEntry.Text))
                    {
                        TitleEntry.FieldError = "Title must not be empty!";   
                        return;
                    }
                    
                    if (currentElemIndex == candidateEntries.Count - 1 && (!string.IsNullOrWhiteSpace(TitleEntry.Text) || !string.IsNullOrWhiteSpace(PartyEntry.Text)))
                    {
                        _issuedPage.CandidateEntries.Add(new CandidateCreatorItemViewModel(_issuedPage));
                        return;
                    }


                }
            }
        }


        
        public CandidateOption GenerateCandidate()
        {
            return new CandidateOption
            {
                Title = TitleEntry.Text,
                Party = PartyEntry.Text
            };
        }
    }
}