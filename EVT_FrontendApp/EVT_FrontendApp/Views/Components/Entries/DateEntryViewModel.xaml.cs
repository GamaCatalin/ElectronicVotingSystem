using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVT_FrontendApp.Views.Entries
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateEntryView : ContentView
    {
        public DateEntryView()
        {
            InitializeComponent();
            // EntInput.Focused += EntInput_Focused;
            // EntInput.Unfocused += EntInput_Focused;
        }

        private void EntInput_Focused(object sender, FocusEventArgs e)
        {
            // if (e.IsFocused)
            // {
            //     await Task.WhenAll(BvUnderline.FadeTo(1),
            //         GrdUnderline.ScaleXTo(1));
            // }
            // else
            // {
            //     await Task.WhenAll(BvUnderline.FadeTo(0.5),
            //         GrdUnderline.ScaleXTo(0));
            // }
        }
    }
}