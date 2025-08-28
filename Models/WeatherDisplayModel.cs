namespace WTForecast.Models
{
    public class WeatherDisplayModel
    {
        public string ConditionText { get; set; }
        public string TemperatureText { get; set; }
        public string WindText { get; set; }
        public string IconPath { get; set; }
        public double WindRotation { get; set; }
    }
}
