namespace WTForecast
{
    public class ShortWeatherData
    {
        public List<ShortWeather> shortWeather { get; set; }
        public double minTemperature { get; set; }
        public double maxTemperature { get; set; }
        public double maxRain { get; set; }
    }

    public class ShortWeather
    {
        public DateTime dateTime { get; set; }
        public double temperature { get; set; }
        public double windSpeed { get; set; }
        public double precipitationAmount { get; set; }
        public string symbolCode { get; set; }
    }

    public class DailyWeatherData
    {
        public DateTime date { get; set; }
        public double minTemperature { get; set; }
        public double maxTemperature { get; set; }
        public double maxRain { get; set; }

    }
}
