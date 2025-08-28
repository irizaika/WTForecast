using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WTForecast.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public WeatherService(IConfiguration config)
    {
        _httpClient = new HttpClient();
    
        var userAgent = config["WeatherApi:UserAgent"] ?? "WTForecast/1.0 (https://github.com/yourusername/WTForecast; contact@example.com)";

        _baseUrl = config["WeatherApi:BaseUrl"] ?? "https://api.met.no/weatherapi/locationforecast/2.0";

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
    }

    public async Task<WeatherData?> GetWeatherDataAsync(string lat, string lon)
    {
        var url = $"{_baseUrl}/compact?lat={lat}&lon={lon}";
        var response = await _httpClient.GetStringAsync(url);

        try
        {
            return JsonConvert.DeserializeObject<WeatherData>(response);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
