using SkiaSharp.Views.Maui;
using Microcharts;
using WTForecast.Services;
using WTForecast.ViewModels;

/* TODO 
 * improve UI, 
 * add notifications in the morning, in the evening to tomorrow and whe rain starts, 
 * add reload data on background
 * Add logo for daily forecast
 */

namespace WTForecast.Views
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel ViewModel;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = ViewModel = vm;
            _ = ViewModel.LoadWeatherDataAsync();

            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.HourlyTemperatureChart))
                {
                    WeatherChart.InvalidateSurface();
                }

                if (e.PropertyName == nameof(ViewModel.HourlyRainChart))
                {
                    RainChart.InvalidateSurface();
                }

                if (e.PropertyName == nameof(ViewModel.DailyTemperatureRanges))
                {
                    DailyMinMaxChart.InvalidateSurface();
                }

                if (e.PropertyName == nameof(ViewModel.DailyRainAmount))
                {
                    DailyRainChart.InvalidateSurface();
                }
            };
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if ((BindingContext as MainPageViewModel)?.HourlyTemperatureChart is Chart chart)
            {
                var canvas = e.Surface.Canvas;
                canvas.Clear();
                chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }

        private void OnRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (ViewModel.HourlyRainChart is Chart chart)
            {
                var canvas = e.Surface.Canvas;
                canvas.Clear();
                chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }

        private void OnMinMaxDailyCanvasViewPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            var data = (BindingContext as MainPageViewModel)?.DailyTemperatureRanges;
            if (data != null)
            {
                DailyChartDrawer.DrawTemperatureRange(e.Surface.Canvas, e.Info, data.ToList());
            }

        }

        private void OnDailyRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var data = (BindingContext as MainPageViewModel)?.DailyRainAmount;
            if (data != null)
            {
                DailyChartDrawer.DrawRainAmount(e.Surface.Canvas, e.Info, data.ToList());
            }
        }
    }
}
