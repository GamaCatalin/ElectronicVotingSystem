using System;
using EVT_FrontendApp.Views.SKViews.Base;
using SkiaSharp;

namespace EVT_FrontendApp.Views.SKViews
{
    public class CircleGradientView : GradientViewBase
    {
        protected override void DrawGradient(SKImageInfo info, SKCanvas canvas, SKPaint paint)
        {
            var radius = Math.Min(info.Width, info.Height) / 2;
            canvas.DrawCircle(
                info.Width / 2,
                info.Height / 2,
                radius,
                paint
                );
        }
    }
}