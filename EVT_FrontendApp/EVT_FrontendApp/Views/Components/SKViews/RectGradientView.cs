using EVT_FrontendApp.Views.SKViews.Base;
using SkiaSharp;

namespace EVT_FrontendApp.Views.SKViews
{
    public class RectGradientView : GradientViewBase
    {
        protected override void DrawGradient(SKImageInfo info, SKCanvas canvas, SKPaint paint)
        {
            canvas.DrawRect(0, 0, info.Width, info.Height, paint);
        }
    }
}