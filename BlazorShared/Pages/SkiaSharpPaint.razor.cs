using Microsoft.AspNetCore.Components.Web;
using SkiaSharp.Views.Blazor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlazorShared.Pages
{
    public partial class SkiaSharpPaint
    {
        SKCanvasView skiaView = null!;
        SKPath? drawing;

        List<SKPath> drawings = new List<SKPath>();

        private void Log(string prefix, TouchEventArgs e)
        {
            var touch = e.Touches.FirstOrDefault();



            if (touch == null)
                Log($"{prefix}-Touches: ");
            else
                Log($"{prefix}-Touches: Client: {touch.ClientX}, {touch.ClientY}," +
                $" Screen: {touch.ScreenX}, {touch.ScreenY}," +
                $" Page: {touch.PageX}, {touch.PageY}");
        }
        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            // the the canvas and properties
            var canvas = e.Surface.Canvas;

            // make sure the canvas is blank
            canvas.Clear(SKColors.White);

            using var font = new SKFont
            {
                Size = 24
            };
            // decide what the text looks like
            using var paint = new SKPaint(font)
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center
            };


            // draw some text
            var coord = new SKPoint(e.Info.Width / 2, (e.Info.Height + font.Size) / 2);
            //canvas.DrawText("SkiaSharp", coord, SKTextAlign.Center, font, paint);
            canvas.DrawText("SkiaSharp", coord, paint);

            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 5;

            foreach (SKPath drawing in drawings)
                canvas.DrawPath(drawing, paint);

            // draw the path
            if (drawing != null)
                canvas.DrawPath(drawing, paint);
        }

        void OnMouseDown(MouseEventArgs e)
        {
           // StartPaint(e.OffsetX, e.OffsetY);
        }
        void OnTouchStart(TouchEventArgs e)
        {            
            Log($"OnTouchStart", e);
           // StartPaint(e.Touches.FirstOrDefault()?.ClientX, e.Touches.FirstOrDefault()?.ClientY);
        }

        void OnMouseMove(MouseEventArgs e)
        {
            //ContinuePaint(e.OffsetX, e.OffsetY);
        }
        void OnTouchMove(TouchEventArgs e)
        {
            Log($"OnTouchMove", e);
           // ContinuePaint(e.Touches.FirstOrDefault()?.ClientX, e.Touches.FirstOrDefault()?.ClientY);
        }

        void OnMouseUp(MouseEventArgs e)
        {
            //StopPaint(e.OffsetX, e.OffsetY);
        }
        void OnTouchEnd(TouchEventArgs e)
        {
            Log($"OnTouchEnd", e);
            //StopPaint(e.Touches.FirstOrDefault()?.ClientX, e.Touches.FirstOrDefault()?.ClientY);
        }

        void OnPointerDown(PointerEventArgs e)
        {
            StartPaint(e.OffsetX, e.OffsetY);
        }
        void OnPointerMove(PointerEventArgs e)
        {
            ContinuePaint(e.OffsetX, e.OffsetY);
        }
        void OnPointerUp(PointerEventArgs e)
        {
            StopPaint(e.OffsetX, e.OffsetY);
        }


        private void StartPaint(double? x, double? y)
        {
            if (!x.HasValue || !y.HasValue)
                return;

            var point = new SKPoint((float)x, (float)y);

            drawing = new SKPath();
            drawing.MoveTo(point);

            skiaView.Invalidate();
        }

        private void ContinuePaint(double? x, double? y)
        {
            if (drawing is null || !x.HasValue || !y.HasValue)
                return;

            var point = new SKPoint((float)x, (float)y);
            drawing.LineTo(point);

            skiaView.Invalidate();
        }

        private void StopPaint(double? x, double? y)
        {
            if (drawing is null || !x.HasValue || !y.HasValue)
                return;

            var point = new SKPoint((float)x, (float)y);
            drawing.LineTo(point);
            drawings.Add(drawing);

            drawing = null;

            skiaView.Invalidate();
        }
    }
}
