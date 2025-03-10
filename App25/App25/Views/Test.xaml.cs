using System;
using System.IO;
using System.Threading.Tasks;
using App25.Services;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App25.Views
{
    public partial class Test : ContentPage
    {
        private readonly AudioLoader audioLoader = new AudioLoader();

        private float _sliderValue = 0f; // Value between 0 and 1
        private float _thumbX; // Position of thumb
        private readonly float _trackStart = 50;
        private readonly float _trackEnd = 1000;
        private readonly float _thumbSize = 30; // Size of the square

        public Test()
        {
            audioLoader = new AudioLoader();
            InitializeComponent();
            _thumbX = _trackStart;
        }

        private void OnCanvasPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.DrawText("Hello", info.Width / 2, info.Height / 2, new SKPaint { Color = SKColors.Black, TextSize = 100 });

            canvas.Clear(SKColors.White);

            using (var trackPaint = new SKPaint { Color = SKColors.Gray, StrokeWidth = 5, IsAntialias = true })
            using (var thumbPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true })
            {
                canvas.DrawLine(_trackStart, info.Height / 2, _trackEnd, info.Height / 2, trackPaint);

                // Draw square instead of circle
                float squareHalfSize = _thumbSize / 2;
                canvas.DrawRect(_thumbX - squareHalfSize, (info.Height / 2) - squareHalfSize, _thumbSize, _thumbSize, thumbPaint);
            }
        }

        private void OnTouchHandler(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Moved || e.ActionType == SKTouchAction.Pressed)
            {
                _thumbX = Math.Max(_trackStart, Math.Min(e.Location.X, _trackEnd));
                _sliderValue = (_thumbX - _trackStart) / (_trackEnd - _trackStart);
                sliderValueLabel.Text = $"Value: {_sliderValue:F2}";

                sliderCanvas.InvalidateSurface();
                e.Handled = true;
            }
        }


        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    audioLoader.LoadAudio("App25.assets.audio.metal.mp3");
        //    audioLoader.Play();
        //}

        //private void Button_Clicked_2(object sender, EventArgs e)
        //{
        //    audioLoader.Stop();
        //}


        //private async void Button_Clicked_1(object sender, EventArgs e)
        //{
        //    await Shell.Current.GoToAsync("about");
        //}

        //private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    audioLoader.SetVolume(e.NewValue);
        //}

    }
}
