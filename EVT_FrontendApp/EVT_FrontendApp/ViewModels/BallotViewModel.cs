using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class BallotViewModel : BaseViewModel
    {
        private Ballot _selectedBallot;
        
        private CandidateOption _selectedItem;
        public ObservableCollection<CandidateOption> Items { get; }
        
        public Command<CandidateOption> ItemTapped { get; set; }

        private string itemId;
        
        public BallotViewModel()
        {
            Title = "Elections";
            Items = new ObservableCollection<CandidateOption>();

            ItemTapped = new Command<CandidateOption>(OnItemSelected);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                _selectedBallot = await ElectionDataStore.GetItemAsync(itemId);
                Items.Clear();
                foreach (var candidate in _selectedBallot.Options)
                {
                    Items.Add(candidate);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        
        public CandidateOption SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        
        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }
        
        private async void OnItemSelected(CandidateOption item)
        {
            if (item == null)
                return;

            _selectedItem = item;
            // This will push the BallotPage onto the navigation stack
            if (!await VoteService.CastVote(_selectedBallot.Id, _selectedItem.Id))
            {
                await AlertService.AlertAsync("Vote failed!");
            }   
            await Shell.Current.GoToAsync("..");
        }
        
    }
}