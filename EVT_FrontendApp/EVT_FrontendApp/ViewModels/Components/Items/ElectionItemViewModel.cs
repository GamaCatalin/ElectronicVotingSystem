using System;
using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Pages.Election.Ongoing;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Components.Items
{
    public class ElectionItemViewModel : ExtendedBindableObject
    {
        private Ballot _ballot;
        
        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }
        
        
        private string _timeLeft = String.Empty;
        public string TimeLeft
        {
            get => _timeLeft;
            set => SetProperty(ref _timeLeft, value);
        }


        private string _electionName;
        public string ElectionName
        {
            get => _electionName;
            set => SetProperty(ref _electionName, value);
        }
                
        
        private ICommand _tapCommand;
        private readonly INavigationService _navigationService;

        public ICommand TapCommand
        {
            get => _tapCommand;
            set => SetProperty(ref _tapCommand, value);
        }


        public ElectionItemViewModel(Ballot ballot, INavigationService navigationService)
        {
            _ballot = ballot;
            _navigationService = navigationService;

            
            TimeLeft = GetTimeLeftString();
            
            ElectionName = ballot.ElectionName;

            TapCommand = OnItemSelected();

        }

        private ICommand OnItemSelected()
        {
            return new Command(NavigateToBallotPage);
        }

        private async void NavigateToBallotPage(object obj)
        {
            await _navigationService.NavigateToAsync<BallotViewModel>(_ballot);
        }

        private string GetTimeLeftString()
        {
            var str = string.Empty;

            var timeLeft = _ballot.EndDate - DateTime.Now;
            var timeLeftDays = timeLeft.Days;

            if (timeLeftDays <= 0)
            {
                str = "Election overdue!";
                _isAvailable = false;
            }
            else
            {
                if (timeLeftDays == 1)
                {
                    str = "There is " + timeLeftDays + " day remaining!";
                }
                else
                {
                    str = "There are " + timeLeftDays + " days remaining!";
                }

                _isAvailable = true;
            }
            
            return str;
        }
    }
}