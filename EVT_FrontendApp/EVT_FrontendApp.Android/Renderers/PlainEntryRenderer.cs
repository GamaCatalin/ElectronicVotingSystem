using Android.Content;
using EVT_FrontendApp.Droid.Renderers;
using EVT_FrontendApp.Views.Entries;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PlainEntry), typeof(PlainEntryRenderer))]
namespace EVT_FrontendApp.Droid.Renderers
{
    public class PlainEntryRenderer : EntryRenderer
    {
        public PlainEntryRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackground(null);
                Control.SetPadding(0,0,0,0);
            }
        }
    }
}