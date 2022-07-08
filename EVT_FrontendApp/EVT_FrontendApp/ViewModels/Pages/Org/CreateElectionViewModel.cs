using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Election;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.Services.Org;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;
using EVT_FrontendApp.ViewModels.Items;
using EVT_FrontendApp.Views.Entries;
using EVT_FrontendApp.Views.Items;

namespace EVT_FrontendApp.ViewModels.Pages.Org
{
    public class CreateElectionViewModel : BaseViewModel
    {
        public bool IsInUse;

        public TextEntryViewModel ElectionNameEntryViewModel { get; set; }
        public DateEntryViewModel EndDateEntryViewModel { get; set; }

        public ObservableCollection<CandidateCreatorItemViewModel> CandidateEntries { get; set; }

        public ButtonModel CreateElectionModel { get; set; }


        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IAlertService _alertService;
        private IElectionService _electionService;


        public CreateElectionViewModel(IAlertService alertService, IAccountService accountService,
            INavigationService navigationService, IElectionService electionService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _alertService = alertService;
            _electionService = electionService;


            ElectionNameEntryViewModel = new TextEntryViewModel("Election Name");
            ElectionNameEntryViewModel.PropertyChanged += OnElectionNameTextChanged;

            EndDateEntryViewModel = new DateEntryViewModel();
            EndDateEntryViewModel.PropertyChanged += OnDateSelectionChanged;

            CandidateEntries = new ObservableCollection<CandidateCreatorItemViewModel>();
            CandidateEntries.CollectionChanged += OnCandidatesCollectionChanged;
            CandidateEntries.Add(new CandidateCreatorItemViewModel(this));

            CreateElectionModel = new ButtonModel("CREATE ELECTION", OnCreateElectionAction);
        }

        private void OnCandidatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateIndexLabels();
        }

        private void OnElectionNameTextChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                ElectionNameEntryViewModel.FieldError = "";
            }
        }

        private void OnDateSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                EndDateEntryViewModel.FieldError = "";
                var selectedDate = EndDateEntryViewModel.Date;
                if (selectedDate < DateTime.Today)
                {
                    EndDateEntryViewModel.FieldError = "Invalid date!";
                }
            }
        }


        private ObservableCollection<CandidateOption> CollectCandidates()
        {
            var candidates = new ObservableCollection<CandidateOption>();
            foreach (var candidateItem in CandidateEntries)
            {
                var candidate = (CandidateOption) candidateItem.GenerateCandidate();
                if (!string.IsNullOrWhiteSpace(candidate.Title))
                {
                    candidates.Add(candidate);
                }
            }

            return candidates;
        }

        private async void OnCreateElectionAction()
        {
            if (!await ValidateElectionName())
            {
                return;
            }

            var candidates = CollectCandidates();

            if (!await ValidateCandidates(candidates))
            {
                return;
            }

            IsBusy = true;


            var election = await CreateElection(candidates);

            var addOrganizationException = await _electionService.AddElectionAsync(election);

            if (addOrganizationException)
            {
                // await _navigationService.GoBackAsync();
                await _navigationService.NavigateToAsync<DashboardViewModel>();
            }
            else
            {
                await _alertService.AlertAsync("Election creation failed!");
            }

            IsBusy = false;
        }

        private async Task<bool> ValidateCandidates(ObservableCollection<CandidateOption> candidates)
        {
            if (candidates.Count < 2)
            {
                await _alertService.AlertAsync("Must have at least two valid candidates!");
                return false;
            }

            ResetDuplicateTitleErrors();

            Dictionary<string, List<int>> candidateTitles = new Dictionary<string, List<int>>();
            var foundDuplicates = false;

            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidateTitles.ContainsKey(candidates[i].Title))
                {
                    foundDuplicates = true;
                    candidateTitles[candidates[i].Title].Add(i);
                }
                else
                {
                    candidateTitles.Add(candidates[i].Title, new List<int>(new[] {i}));
                }
            }

            if (foundDuplicates)
            {
                ShowDuplicateTitleErrors(candidateTitles);
                return false;
            }


            return true;
        }

        private void ShowDuplicateTitleErrors(Dictionary<string, List<int>> candidateTitles)
        {
            var positionLists = candidateTitles.Values;
            var duplicatePositions = new List<int>();


            foreach (var positionList in positionLists)
            {
                if (positionList.Count > 1)
                {
                    duplicatePositions.AddRange(positionList);
                }
            }


            foreach (var duplicatePosition in duplicatePositions)
            {
                CandidateEntries[duplicatePosition].TitleEntry.FieldError = "Duplicate title!";
            }
        }

        private void ResetDuplicateTitleErrors()
        {
            foreach (var candidateItem in CandidateEntries)
            {
                candidateItem.TitleEntry.FieldError = "";
            }
        }

        private async Task<Ballot> CreateElection(ObservableCollection<CandidateOption> candidates)
        {
            var electionName = ElectionNameEntryViewModel.Text;
            var creator = await _accountService.GetUserAsync();


            return new Ballot
            {
                ElectionName = electionName,
                StartDate = DateTime.Now,
                EndDate = EndDateEntryViewModel.Date,
                Options = candidates,
                OrganizationId = creator.OrganizationId
            };
        }

        private Task<bool> ValidateElectionName()
        {
            var orgName = ElectionNameEntryViewModel.Text;
            ElectionNameEntryViewModel.FieldError = "";

            if (string.IsNullOrWhiteSpace(orgName))
            {
                ElectionNameEntryViewModel.FieldError = "Election name must not be empty!";
                // await _alertService.AlertAsync("Election name must not be empty!");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void UpdateIndexLabels()
        {
            for (var i = 0; i < CandidateEntries.Count; i++)
            {
                CandidateEntries[i].ItemIndex = (i + 1) + ".";
            }
        }
    }
}