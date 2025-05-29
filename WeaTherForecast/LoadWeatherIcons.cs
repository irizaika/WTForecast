/*
    ONE TIME Method to load weather icons from OpenWeatherMap
*/
namespace WTForecast
{
    class LoadWeatherIcons
    {
        static readonly Dictionary<string, string> symbolToIconCode = new()
        {
            { "ClearskyDay", "01d" },
            { "ClearskyNight", "01n" },
            { "Cloudy", "03d" },                // scattered clouds
            { "FairDay", "02d" },               // few clouds (day)
            { "FairNight", "02n" },             // few clouds (night)
            { "Fog", "50d" },                   // mist
            { "Heavyrain", "10d" },             // rain (day)
            { "HeavyrainAndThunder", "11d" },  // thunderstorm
            { "HeavyrainshowersDay", "09d" },  // shower rain
            { "HeavyrainshowersNight", "09n" },
            { "Heavysleet", "13d" },            // snow (closest)
            { "HeavysleetAndThunder", "11d" },
            { "HeavysleetshowersDay", "13d" },
            { "HeavysleetshowersNight", "13n" },
            { "Heavysnow", "13d" },
            { "HeavysnowAndThunder", "11d" },
            { "HeavysnowshowersDay", "13d" },
            { "HeavysnowshowersNight", "13n" },
            { "PartlycloudyDay", "03d" },
            { "PartlycloudyNight", "03n" },
            { "Rain", "10d" },
            { "RainAndThunder", "11d" },
            { "RainshowersDay", "09d" },
            { "RainshowersNight", "09n" },
            { "Sleet", "13d" },
            { "SleetAndThunder", "11d" },
            { "SleetshowersDay", "13d" },
            { "SleetshowersNight", "13n" },
            { "Snow", "13d" },
            { "SnowAndThunder", "11d" },
            { "SnowshowersDay", "13d" },
            { "SnowshowersNight", "13n" },
            { "ShowersDay", "09d" },
            { "ShowersNight", "09n" },
            { "Thunderstorm", "11d" },
            { "ThunderstormLight", "11d" }
        };

        async void Load()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "WeatherIcons");
            Directory.CreateDirectory(folderPath);

            using var client = new HttpClient();

            foreach (var kvp in symbolToIconCode)
            {
                string symbol = kvp.Key.ToLower();
                string iconCode = kvp.Value;

                try
                {
                    string url = $"http://openweathermap.org/img/wn/{iconCode}@2x.png";
                    byte[] data = await client.GetByteArrayAsync(url);
                    string filePath = Path.Combine(folderPath, $"{symbol}.png");
                    await File.WriteAllBytesAsync(filePath, data);
                    Console.WriteLine($"Downloaded icon for {symbol} ({iconCode})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to download icon for {symbol}: {ex.Message}");
                }
            }

            Console.WriteLine("All downloads complete!");
        }
    }
}
