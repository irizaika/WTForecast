using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using WTForecast.Models;
using WTForecast.Services;

namespace WTForecast.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly WeatherFacade _weatherFacade;
    private readonly WeatherMapper _weatherMapper;
    private readonly ChartService _chartService;

    [ObservableProperty] 
    string _conditionText;
    [ObservableProperty] 
    string _temperatureText;
    [ObservableProperty] 
    string _windText;
    [ObservableProperty] 
    double _windRotation;
    [ObservableProperty] 
    string _iconPath;
    [ObservableProperty] 
    bool _iconVisible;
    [ObservableProperty]
    bool _windRotationVisible;

    [ObservableProperty] 
    Chart _hourlyTemperatureChart;
    [ObservableProperty] 
    Chart _hourlyRainChart;

    [ObservableProperty]
    List<DailyRainAmountData> _dailyRainAmount = new();

    [ObservableProperty]
    List<DailyTemperatureRangeData> _dailyTemperatureRanges = new();

    [ObservableProperty]
    bool _isLoading;

    [ObservableProperty]
    bool _isRefreshing;

    public MainPageViewModel(WeatherFacade weatherFacade, WeatherMapper weatherMapper,
        ChartService chartService)
    {
        _weatherFacade = weatherFacade;
        _weatherMapper = weatherMapper;
        _chartService = chartService;
    }

    [RelayCommand]
    public async Task LoadWeatherDataAsync()
    {
        try
        {
            IsLoading = true;
            var (weatherData, todaysWeather) = await _weatherFacade.LoadWeatherAsync();
            if (weatherData == null || todaysWeather == null) return;

            // Labels
            var displayModel = _weatherMapper.BuildDisplayModel(weatherData);
            ConditionText = displayModel.ConditionText;
            TemperatureText = displayModel.TemperatureText;
            IconPath = displayModel.IconPath;
            IconVisible = true;
            WindText = displayModel.WindText;
            WindRotation = (double)displayModel.WindRotation;
            WindRotationVisible = true;

            DailyTemperatureRanges = _weatherMapper.GetDailyTemperatureRangeData(weatherData);

            DailyRainAmount = _weatherMapper.GetDailyRainAmountData(weatherData);

            HourlyTemperatureChart = _chartService.CreateHourlyTemperatureChart(
                _weatherMapper.GetTodaysHourlyTemperatureChartEnties(todaysWeather),
                todaysWeather.minTemperature, todaysWeather.maxTemperature);

            HourlyRainChart = _chartService.CreateHourlyRainChart(
                _weatherMapper.GetTodaysHourlyRainChartEnties(todaysWeather),
                todaysWeather.maxRain);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task ReloadWeatherAsync()
    {
        try
        {
            IsRefreshing = true;
            await LoadWeatherDataAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
