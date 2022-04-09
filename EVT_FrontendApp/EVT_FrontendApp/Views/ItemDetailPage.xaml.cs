using EVT_FrontendApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace EVT_FrontendApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}