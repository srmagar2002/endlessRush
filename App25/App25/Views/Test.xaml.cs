using System;
using System.IO;
using System.Threading.Tasks;
using App25.Services;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;

namespace App25.Views
{
    public partial class Test : ContentPage, INotifyPropertyChanged
    {
        private readonly AudioLoader audioLoader = new AudioLoader();

        private float _sliderValue = 0f; // Value between 0 and 1
        private float _thumbX; // Position of thumb
        private readonly float _trackStart = 50;
        private readonly float _trackEnd = 1000;
        private readonly float _thumbSize = 30; // Size of the square

        public event PropertyChangedEventHandler PropertyChanged;
        public float SliderValue
        {
            get => _sliderValue;
            set
            {
                if (_sliderValue != value)
                {
                    _sliderValue = value;
                    OnPropertyChanged(nameof(SliderValue));
                    OnSliderValueChanged(_sliderValue); // Call function when value changes
                }
            }
        }
        public Test()
        {
            audioLoader = new AudioLoader();
            _thumbX = _trackEnd;
            InitializeComponent();
        }

        private void OnCanvasPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;



            canvas.Clear(SKColors.White);

            using (var trackPaint = new SKPaint { Color = SKColors.Gray, IsAntialias = true })
            using (var thumbPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true })
            {
                //canvas.DrawLine(_trackStart, info.Height / 2, _trackEnd, info.Height / 2, trackPaint);

                float trackHeight = 30;
                canvas.DrawRect(_trackStart, (info.Height / 2) - (trackHeight / 2), _trackEnd - _trackStart, trackHeight, trackPaint);

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
                SliderValue = (_thumbX - _trackStart) / (_trackEnd - _trackStart);
                sliderValueLabel.Text = $"Value: {_sliderValue:F2}";

                sliderCanvas.InvalidateSurface();
                e.Handled = true;
            }
        }

        private void OnSliderValueChanged(float volume)
        {
            audioLoader.SetVolume(volume);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
