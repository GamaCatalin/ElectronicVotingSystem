using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp.Views.Labels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandidatePartyLabel : Label
    {
        public CandidatePartyLabel()
        {
            InitializeComponent();
        }
    }
}