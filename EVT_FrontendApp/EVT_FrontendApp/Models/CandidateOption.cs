using System;
using Xamarin.Forms;

namespace EVT_FrontendApp.Models
{
    public class CandidateOption
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public string Party { get; set; }
        public int VoteCount { get; set; } = 0;
    }
}