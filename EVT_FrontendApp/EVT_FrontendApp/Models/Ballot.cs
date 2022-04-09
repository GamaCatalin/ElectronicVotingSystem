using System.Collections;
using System.Collections.Generic;

namespace EVT_FrontendApp.Models
{
    public class Ballot
    {
        public string Id { get; set; }
        public string ElectionName { get; set; }
        public IEnumerable<CandidateOption> Options { get; set; }

        public bool HasVoted { get; set; } = false;
    }
}