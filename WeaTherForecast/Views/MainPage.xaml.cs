using SkiaSharp.Views.Maui;
using Microcharts;
using SkiaSharp.Views.Maui.Controls;
using WTForecast.Models;
using WTForecast.Services;

/* TODO 
 * move code to pagemodel?
 * add amination while data loadig, 
 * improve UI, 
 * add notifications in the morning, in the evening to tomorrow and whe rain starts, 
 * add reload data on background
 * Add logo for daily forecast
 */

namespace WTForecast
{
    public partial class MainPage : ContentPage
    {
        // For hourly temperature and rain display - 'out of the box' chart used
        Chart _hourlyTemperatureChart;
        Chart _hourlyRainChart;
        List<ChartEntry> _hourlyTemperatureEntries = new List<ChartEntry>();
        List<ChartEntry> _hourlyRainEntries = new List<ChartEntry>();

        // For daily temperature and rain display - canvas with Bezier vurves used 
        List<DailyRainAmountData> _dailyRainAmountData = new List<DailyRainAmountData>();
        List<DailyTemperatureRangeData> _dailyTemperatureRangeData = new List<DailyTemperatureRangeData>();

        double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        private readonly WeatherMapper _weatherMapper;
        private readonly ChartService _chartService;
        private readonly DailyChartDrawer _dailyChartDrawer;
        private readonly WeatherFacade _weatherFacade;

        public MainPage(WeatherFacade weatherFacade, WeatherMapper weatherMapper,
            ChartService chartService, DailyChartDrawer dailyChartDrawer)
        {
            _weatherFacade = weatherFacade;
            _weatherMapper = weatherMapper;
            _chartService = chartService;
            _dailyChartDrawer = dailyChartDrawer;

            InitializeComponent();
            LoadWeatherData();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_hourlyTemperatureChart == null) return;

            var canvas = e.Surface.Canvas;
            canvas.Clear(); // Optional: clear before drawing

            _hourlyTemperatureChart.Draw(canvas, e.Info.Width, e.Info.Height);
        }

        private void OnRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_hourlyRainChart == null) return;

            var canvas = e.Surface.Canvas;
            canvas.Clear(); // Optional: clear before drawing

            _hourlyRainChart.Draw(canvas, e.Info.Width, e.Info.Height);
        }

        private void BuildCharts(ShortWeatherData todaysWeather, WeatherData weatherData)
        {
            _dailyTemperatureRangeData = _weatherMapper.GetDailyTemperatureRangeData(weatherData);
            _dailyRainAmountData = _weatherMapper.GetDailyRainAmountData(weatherData);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DailyMinMaxChart.InvalidateSurface();
                DailyRainChart.InvalidateSurface();
            });

            _hourlyTemperatureEntries = _weatherMapper.GetTodaysHourlyTemperatureChartEnties(todaysWeather);
            _hourlyRainEntries = _weatherMapper.GetTodaysHourlyRainChartEnties(todaysWeather);

            UpdateHourlyWeatherChart(WeatherChart, _hourlyTemperatureEntries, todaysWeather.minTemperature, todaysWeather.maxTemperature);
            UpdateHourlyRainChart(RainChart, _hourlyRainEntries, 0, todaysWeather.maxRain);
        }

        private void UpdateUI(WeatherData weatherData)
        {
            SetTodaysWeatherLabels(weatherData);
        }

        private async void LoadWeatherData()
        {
            try
            {
                var (weatherData, todaysWeather) = await _weatherFacade.LoadWeatherAsync();
                if (weatherData == null || todaysWeather == null) return;

                BuildCharts(todaysWeather, weatherData);

                UpdateUI(weatherData);
            }
            catch (Exception ex)
            {
                // TODO: log error or show message
            }
        }

        async void UpdateHourlyWeatherChart(SKCanvasView chart, List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            _hourlyTemperatureChart = _chartService.CreateHourlyTemperatureChart(entries, minTemp, maxTemp);

            chart.IsVisible = true;
            chart.HeightRequest = 250;
            // chart.WidthRequest = screenWidth;

            await Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                chart.InvalidateSurface();
            });
        }

        async void UpdateHourlyRainChart(SKCanvasView chart, List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            _hourlyRainChart = _chartService.CreateHourlyRainChart(entries, maxTemp);

            chart.IsVisible = true;
            chart.HeightRequest = 50;
            // chart.WidthRequest = screenWidth;

            await Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                chart.InvalidateSurface();
            });
        }


        private void SetTodaysWeatherLabels(WeatherData weatherData)
        {
            var displayModel = _weatherMapper.BuildDisplayModel(weatherData);

            ConditionLabel.Text = displayModel.ConditionText;
            TemperatureLabel.Text = displayModel.TemperatureText;
            WeatherIcon.Source = displayModel.IconPath;
            WeatherIcon.IsVisible = true;
            WindLabel.Text = displayModel.WindText;
            WindArrow.Rotation = (double)displayModel.WindRotation;
            WindArrow.IsVisible = true;
            // MessageLabel.Text = condition?.GetFunnyMessage() ?? "Unknown weather condition";
        }

        private void OnMinMaxDailyCanvasViewPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            _dailyChartDrawer.DrawTemperatureRange(e.Surface.Canvas, e.Info, _dailyTemperatureRangeData);
        }

        private void OnDailyRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _dailyChartDrawer.DrawRainAmount(e.Surface.Canvas, e.Info, _dailyRainAmountData);
        }
    }
}
