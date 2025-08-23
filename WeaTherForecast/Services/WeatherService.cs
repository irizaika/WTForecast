using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace WTForecast.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("WTForecast", "1.0"));
    }

    public async Task<WeatherData?> GetWeatherDataAsync(string lat, string lon)
    {
        var url = $"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={lat}&lon={lon}";
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
