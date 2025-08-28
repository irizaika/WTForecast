using Microcharts;
using SkiaSharp;
using WTForecast.Models;
using WTForecast.Utils;

namespace WTForecast.Services
{
    public class WeatherMapper
    {

        public ShortWeatherData GetTodayWeatherValues(WeatherData weatherData)
        {

            List<ShortWeather> shortWeathers = weatherData.properties.timeseries.Select(ts => new ShortWeather
            {
                dateTime = ts.time.ToLocalTime(),
                temperature = ts.data.instant.details.air_temperature,
                windSpeed = ts.data.instant.details.wind_speed,
                precipitationAmount = ts.data.next_1_hours.details.precipitation_amount,
                symbolCode = ts.data.next_1_hours.summary?.symbol_code ?? "unknown"
            }).Take(24).ToList();

            var minTemp = shortWeathers.Min(x => x.temperature);
            var maxTemp = shortWeathers.Max(x => x.temperature);
            var maxRain = shortWeathers.Max(x => x.precipitationAmount);

            //foreach (ShortWeather entry in shortWeathers)
            //{
            //    Logger.LogAsync($"entry.temperature = {entry.temperature}, entry.dateTime = {entry.dateTime}");
            //}

            return new ShortWeatherData()
            {
                shortWeather = shortWeathers,
                minTemperature = minTemp,
                maxTemperature = maxTemp,
                maxRain = maxRain,
            };

        }

        public List<ChartEntry> GetTodaysHourlyTemperatureChartEnties(ShortWeatherData todaysWeather)
        {
            var minTemp = todaysWeather.minTemperature;
            var maxTemp = todaysWeather.maxTemperature;
            var sortedWeatherList = todaysWeather.shortWeather;

            return sortedWeatherList.Select((d, index) =>
            {
                var i = 0;
                var temp = d.temperature;
                var color = DisplayUtils.GetTemperatureColor(temp);
                var dateTimeLocal = d.dateTime.ToLocalTime();


                var labelColor = DisplayUtils.Darken(color);

                bool showLabel = temp == minTemp || temp == maxTemp || index == 0 || index == sortedWeatherList.Count - 1 || index % 3 == 0;

                return new ChartEntry((float)temp) // Invert if you're still flipping Y
                {
                    Label = d.dateTime.ToString("HH:mm"),
                    ValueLabel = showLabel ? $"{temp}°C" : null,
                    Color = color,          // Point color
                    ValueLabelColor = labelColor, // Optional: Label matches color

                };
            }).ToList();
        }

        public List<ChartEntry> GetTodaysHourlyRainChartEnties(ShortWeatherData todaysWeather)
        {
            var sortedWeatherList = todaysWeather.shortWeather;

            return sortedWeatherList.Select((d, index) =>
            {
                var precip = d.precipitationAmount;

                // With this for true invisibility in Microcharts (use a fully transparent color with alpha 0):
                var color = precip > 0 ? SKColors.DeepSkyBlue : SKColor.Parse("#00666666");
                bool showLabel = precip > 0;

                return new ChartEntry((float)precip)
                {
                    // Label = d.dateTime.ToString("HH:mm"),
                    ValueLabel = showLabel ? $"{precip} mm" : null,
                    Color = color,
                    ValueLabelColor = color,
                };
            }).ToList();
        }

        public List<DailyTemperatureRangeData> GetDailyTemperatureRangeData(WeatherData weatherData)
        {
            // Group timeseries entries by date, skip today's date
            var today = DateTime.Now.Date;

            return weatherData.properties.timeseries
                .GroupBy(ts => ts.time.Date)//skip today wit have 24 hourly forecats
                .Where(g => g.Key > today)
                .Select(g =>
                {
                    var temps = g.Select(ts => ts.data.instant.details.air_temperature);
                    return new DailyTemperatureRangeData() { date = g.Key, maxTemperature = (float)temps.Max(), minTemperature = (float)temps.Min() };
                }).Take(9)
                .ToList();
        }

        public List<DailyRainAmountData> GetDailyIconData(WeatherData weatherData)
        {
            // Group timeseries entries by date, skip today's date
            var today = DateTime.Now.Date;

            return weatherData.properties.timeseries
                .Where(ts => ts.time.Date > today)
                .GroupBy(ts => ts.time.Date)
                .Select(g =>
                {
                    // Find times at 00:00, 06:00, 12:00, 18:00
                    var sixHourEntries = g.Where(ts =>
                        ts.time.Hour == 0 || ts.time.Hour == 6 || ts.time.Hour == 12 || ts.time.Hour == 18);

                    // Sum precipitation_amount from next_6_hours.details if available, else 0
                    List<string> rainSum = sixHourEntries.Select(ts => ts.data.next_6_hours?.summary?.symbol_code ?? "").ToList();

                    return new DailyRainAmountData
                    {
                        date = g.Key.ToLocalTime(),
                      //  sumRain = (float)rainSum
                    };
                }).Take(9)
                .ToList();
        }


        public List<DailyRainAmountData> GetDailyRainAmountData(WeatherData weatherData)
        {
            var today = DateTime.Now.Date;

            //rain amount, read 6hours deatils
            //0000, 600, 1200, 1800 and take summ of precipitation_amount

            // Rain amount: sum precipitation_amount for 6-hour intervals (00:00, 06:00, 12:00, 18:00)
            return weatherData.properties.timeseries
                .Where(ts => ts.time.Date > today)
                .GroupBy(ts => ts.time.Date)
                .Select(g =>
                {
                    // Find times at 00:00, 06:00, 12:00, 18:00
                    var sixHourEntries = g.Where(ts =>
                        ts.time.Hour == 0 || ts.time.Hour == 6 || ts.time.Hour == 12 || ts.time.Hour == 18);

                    // Sum precipitation_amount from next_6_hours.details if available, else 0
                    double rainSum = sixHourEntries.Sum(ts =>
                        ts.data.next_6_hours?.details?.precipitation_amount ?? 0);

                    return new DailyRainAmountData
                    {
                        date = g.Key.ToLocalTime(),
                        sumRain = (float)rainSum
                    };
                }).Take(9)
                .ToList();
        }

        public WeatherDisplayModel BuildDisplayModel(WeatherData weatherData)
        {
            var currentWeather = weatherData.properties.timeseries[0];
            var temp = currentWeather.data.instant.details.air_temperature;
            var windSpeed = currentWeather.data.instant.details.wind_speed;
            var windDirection = currentWeather.data.instant.details.wind_from_direction;

            string? symbolCode = currentWeather.data.next_1_hours?.summary?.symbol_code;
            var condition = SymbolCodeExtensions.ParseEnumByEnumMemberValue<SymbolCode>(symbolCode);

            return new WeatherDisplayModel
            {
                ConditionText = condition?.GetMeaning() ?? "Unknown",
                TemperatureText = $"{temp}°C",
                WindText = $"Wind {windSpeed} m/s",//to do m/s
                IconPath = condition.ToString().ToLower() + ".png",
                WindRotation = (double)windDirection
            };
        }

        //not used
        private void GetPrecipitation(WeatherData weatherData)
        {
            // Assuming you have a WeatherData instance called weatherData

            var groupedByDate = weatherData.properties.timeseries
                .GroupBy(ts => ts.time.Date);

            foreach (var dateGroup in groupedByDate)
            {
                double nightPrecip = 0;
                double dayPrecip = 0;

                foreach (var ts in dateGroup)
                {
                    // Only use 1-hourly details if available
                    if (ts.data.next_1_hours?.details != null)
                    {
                        var hour = ts.time.Hour;
                        var precip = ts.data.next_1_hours.details.precipitation_amount;

                        if (hour >= 0 && hour < 8)
                        {
                            nightPrecip += precip;
                        }
                        else
                        {
                            dayPrecip += precip;
                        }
                    }
                }

                Console.WriteLine($"Date: {dateGroup.Key:yyyy-MM-dd}");
                Console.WriteLine($"  Night (00-08): {nightPrecip} mm");
                Console.WriteLine($"  Day   (08-00): {dayPrecip} mm");
            }
        }

    }
}
