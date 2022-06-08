using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EVT_FrontendApp.Models
{
    public class ElectionResults
    {
        public string ElectionName { get; set; }
        public CandidateOption Winner { get; set; }

        public string WinnerName => Winner is null ? "" : Winner.Title;

        public ObservableCollection<CandidateOption> Candidates { get; set; }
        public Dictionary<string, int> Results { get; set; }
    }
}