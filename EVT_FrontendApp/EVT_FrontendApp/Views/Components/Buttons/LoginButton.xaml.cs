using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp.Views.Buttons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginButton : BindableButton
    {
        public LoginButton()
        {
            InitializeComponent();
        }
    }
}