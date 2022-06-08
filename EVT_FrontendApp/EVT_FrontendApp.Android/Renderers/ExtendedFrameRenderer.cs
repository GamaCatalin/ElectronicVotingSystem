using EVT_FrontendApp.Droid.Renderers;
using EVT_FrontendApp.Views.Layouts;
using Xamarin.Forms;
using FrameRenderer = Xamarin.Forms.Platform.Android.FastRenderers.FrameRenderer;


[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace EVT_FrontendApp.Droid.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer
    {
        
    }
}