using WTForecast.Models;

namespace WTForecast.Services
{
    public class WeatherFacade
    {
        private readonly WeatherService _weatherService;
        private readonly LocationService _locationService;
        private readonly WeatherMapper _weatherMapper;

        public WeatherFacade(WeatherService weatherService, LocationService locationService, WeatherMapper weatherMapper)
        {
            _weatherService = weatherService;
            _locationService = locationService;
            _weatherMapper = weatherMapper;
        }

        public async Task<(WeatherData? data, ShortWeatherData? today)> LoadWeatherAsync()
        {
            var location = await _locationService.GetLocationAsync();
            if (location == null) return (null, null);

            string lat = location.Latitude.ToString("F4");
            string lon = location.Longitude.ToString("F4");

            var weatherData = await _weatherService.GetWeatherDataAsync(lat, lon);
            if (weatherData == null) return (null, null);

            var today = _weatherMapper.GetTodayWeatherValues(weatherData);
            return (weatherData, today);
        }
    }

}
