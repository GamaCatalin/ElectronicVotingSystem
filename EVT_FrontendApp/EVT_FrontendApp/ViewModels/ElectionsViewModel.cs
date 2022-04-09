using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Views;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels
{
    public class ElectionsViewModel : BaseViewModel
    {
        private Ballot _selectedItem;
        public ObservableCollection<Ballot> Items { get; }

        public Command LoadElectionsCommand { get; set; }

        public Command<Ballot> ItemTapped { get; set; }


        public ElectionsViewModel()
        {
            Title = "Elections";
            Items = new ObservableCollection<Ballot>();
            LoadElectionsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Ballot>(OnItemSelected);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var ballots = await ElectionDataStore.GetItemsAsync(true);
                foreach (var ballot in ballots)
                {
                    if (!ballot.HasVoted)
                    {
                        Items.Add(ballot);                        
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }
        
        public Ballot SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        
        private async void OnItemSelected(Ballot item)
        {
            if (item == null)
                return;

            // This will push the BallotPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(BallotPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}