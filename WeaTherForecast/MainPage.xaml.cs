using System.Net.Http.Headers;
using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Microcharts;
using SkiaSharp.Views.Maui.Controls;

/* TODO 
 * REFACTOR, 
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
        Chart _hourlyTemperatureChart;
        Chart _hourlyRainChart;
        Chart _dailyRainChart;
        Location? _location;
        string? _lon;
        string? _lat;

        List<ChartEntry> _hourlyTemperatureEntries = new List<ChartEntry>();
        List<ChartEntry> _hourlyRainEntries = new List<ChartEntry>();
        List<ChartEntry> _dailyRainEntries = new List<ChartEntry>();

        List<float> _minDailyTemperatures = new() { };
        List<float> _maxDailyTemperatures = new() { };
        List<DateTime> _dailyDates = new() { };
        List<float> _rainSummary = new() { };

        const int margin = 2;
        double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        public MainPage()
        {
            InitializeComponent();
            LoadWeatherData();


            WeatherChart.SizeChanged += (s, e) =>
            {
                WeatherChart.InvalidateSurface();
            };

            //WeatherChart.PaintSurface += (sender, e) =>
            //{
            //    var surface = e.Surface;
            //   // var surfaceWidth = e.Info.Width;
            //  //  var surfaceHeight = e.Info.Height;

            //    var canvas = surface.Canvas;
            //    // draw on the canvas
            //    _chart?.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            //    canvas.Flush();
            //};
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

        //private void OnDailyRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    if (_dailyRainChart == null) return;

        //    var canvas = e.Surface.Canvas;
        //    canvas.Clear(); // Optional: clear before drawing

        //    _dailyRainChart.Draw(canvas, e.Info.Width, e.Info.Height);
        //}


        async void UpdateWeatherChart(SKCanvasView chart, List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            _hourlyTemperatureChart = new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                LineSize = 3,
                PointSize = 8,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelTextSize = 20,
                MinValue = (float)minTemp - margin,
                MaxValue = (float)maxTemp - margin,
                Margin = 5, // Remove extra padding
                LineMode = LineMode.Straight,
            };

            chart.IsVisible = true;
            chart.HeightRequest = 250;
            // chart.WidthRequest = screenWidth;

            await Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                chart.InvalidateSurface();
            });
        }

        async void UpdateRainChart(SKCanvasView chart, List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            _hourlyRainChart = new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                LineSize = 2,
                PointSize = 2,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColors.White,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 10,
                MinValue = (float)minTemp - margin,
                MaxValue = (float)maxTemp - margin,
                Margin = 0, // Remove extra padding
                LineMode = LineMode.Spline,
            };

            chart.IsVisible = true;
            chart.HeightRequest = 50;
            // chart.WidthRequest = screenWidth;

            await Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                chart.InvalidateSurface();
            });
        }

        async void UpdateDailyRainChart(SKCanvasView chart, List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            _dailyRainChart = new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                LineSize = 2,
                PointSize = 2,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColors.White,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 10,
                MinValue = (float)minTemp - margin,
                MaxValue = (float)maxTemp - margin,
                Margin = 0, // Remove extra padding
                LineMode = LineMode.Spline,
            };

            chart.IsVisible = true;
            chart.HeightRequest = 150;
            // chart.WidthRequest = screenWidth;

            await Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                chart.InvalidateSurface();
            });
        }


        private async Task SetLocationAndCoordinates()
        {
            _location = await GetLocationAsync();

            if (_location == null)
            {
                //log error
                return;
            }

            _lat = _location.Latitude.ToString("F4");
            _lon = _location.Longitude.ToString("F4");
            //_lat = "51.5074"; test
            // _lon = "-0.1278"; test
        }

        private async Task<WeatherData> GetWeatherDataFromUrl()
        {
            var url = $"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={_lat}&lon={_lon}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyWeatherApp", "1.0"));

            var response = await httpClient.GetStringAsync(url);
            try
            {
                WeatherData? weatherData = JsonConvert.DeserializeObject<WeatherData>(response);
                if (weatherData != null)
                {
                    return weatherData;
                }
                return new WeatherData();
            }
            catch (JsonException ex)
            {
                // log error
                return new WeatherData();
            }
        }

        private async void LoadWeatherData()
        {
            try
            {
                await SetLocationAndCoordinates();

                if (string.IsNullOrEmpty(_lat) || string.IsNullOrEmpty(_lon))
                {
                    // Show error to user
                    return;
                }

                WeatherData weatherData = await GetWeatherDataFromUrl();

                ShortWeatherData todaysWeather = GetTodayWeatherValues(weatherData);
                List<DailyWeatherData> dailyWeather = GetDailyWeatherValues(weatherData);

                // Filter timeseries where precipitation_amount > 0
                //var precipitationTimes = weatherData.properties.timeseries
                //    .Where(ts => ts.data.instant.details.precipitation_amount > 0)
                //    .ToList();


                SetTodaysWeatherTeperatureEnties(todaysWeather);
                UpdateWeatherChart(WeatherChart, _hourlyTemperatureEntries, todaysWeather.minTemperature, todaysWeather.maxTemperature);
                UpdateRainChart(RainChart, _hourlyRainEntries, 0, todaysWeather.maxRain);
                UpdateDailyRainChart(DailyRainChart, _dailyRainEntries, 0, _rainSummary.Max());

                SetTodaysWeatherLabels(weatherData);
                GetPrecipitation(weatherData);
            }
            catch (Exception ex)
            {
                // MessageLabel.Text = ex.Message;
            }
        }


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

        private ShortWeatherData GetTodayWeatherValues(WeatherData weatherData)
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

            foreach (ShortWeather entry in shortWeathers)
            {
                Logger.LogAsync($"entry.temperature = {entry.temperature}, entry.dateTime = {entry.dateTime}");
            }

            return new ShortWeatherData()
            {
                shortWeather = shortWeathers,
                minTemperature = minTemp,
                maxTemperature = maxTemp,
                maxRain = maxRain,
            };

        }

        private List<DailyWeatherData> GetDailyWeatherValues(WeatherData weatherData)
        {
            // Group timeseries entries by date, skip today's date
            var today = DateTime.Now.Date;
            var groupedByDate = weatherData.properties.timeseries
                .GroupBy(ts => ts.time.Date)//skip today wit have 24 hourly forecats
                .Where(g => g.Key > today)
                .Select(g =>
                {
                    var temps = g.Select(ts => ts.data.instant.details.air_temperature);
                    return new DailyWeatherData() { date = g.Key, maxTemperature = temps.Max(), minTemperature = temps.Min() };
                }).Take(9)
                .ToList();

            //rain amount, read 6hours deatils
            //0000, 600, 1200, 1800 and take summ of precipitation_amount

            // Rain amount: sum precipitation_amount for 6-hour intervals (00:00, 06:00, 12:00, 18:00)
            var dailyRainAmounts = weatherData.properties.timeseries
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

                    return new
                    {
                        Date = g.Key.ToLocalTime(),
                        RainAmount = rainSum
                    };
                }).Take(9)
                .ToList();

            _minDailyTemperatures = groupedByDate.Select(d => (float)d.minTemperature).ToList();
            _maxDailyTemperatures = groupedByDate.Select(d => (float)d.maxTemperature).ToList();
            _dailyDates = groupedByDate.Select(d => d.date).ToList();

            _rainSummary = dailyRainAmounts.Select(d => (float)d.RainAmount).ToList();

            _dailyRainEntries = dailyRainAmounts.Select(d =>
            {
                var color = d.RainAmount > 0 ? SKColors.DeepSkyBlue : SKColor.Parse("#00666666");
                return new ChartEntry((float)d.RainAmount)
                {
                    Label = d.Date.ToString("dd.MM"),
                    ValueLabel = true ? $"{d.RainAmount:0.##} mm" : null,
                    Color = color,
                    //  ValueLabelColor = color,
                };
            }).ToList();

            Task.Delay(1000);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DailyMinMaxChart.InvalidateSurface();
            });

            return groupedByDate;
        }

        private void SetTodaysWeatherTeperatureEnties(ShortWeatherData todaysWeather)
        {
            var minTemp = todaysWeather.minTemperature;
            var maxTemp = todaysWeather.maxTemperature;
            var sortedWeatherList = todaysWeather.shortWeather;

            _hourlyTemperatureEntries = sortedWeatherList.Select((d, index) =>
            {
                var i = 0;
                var temp = d.temperature;
                var color = GetTemperatureColor(temp);
                var dateTimeLocal = d.dateTime.ToLocalTime();

                SKColor Darken(SKColor color, float factor = 1.5f)
                {
                    byte R(byte c) => (byte)Math.Clamp(c * factor, 0, 255);
                    return new SKColor(R(color.Red), R(color.Green), R(color.Blue), color.Alpha);
                }
                var labelColor = Darken(color);

                bool showLabel = temp == minTemp || temp == maxTemp || index == 0 || index == sortedWeatherList.Count - 1 || index % 3 == 0;

                return new ChartEntry((float)temp) // Invert if you're still flipping Y
                {
                    Label = d.dateTime.ToString("HH:mm"),
                    ValueLabel = showLabel ? $"{temp}°C" : null,
                    Color = color,          // Point color
                    ValueLabelColor = labelColor, // Optional: Label matches color

                };
            }).ToList();


            //set daily rain entries
            _hourlyRainEntries = sortedWeatherList.Select((d, index) =>
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
            //set daily rain entries
        }

        private void SetTodaysWeatherLabels(WeatherData weatherData)
        {
            // Access the first timeseries entry
            var currentWeather = weatherData.properties.timeseries[0];

            // Extract values
            var time = currentWeather.time;
            var temp = currentWeather.data.instant.details.air_temperature;
            var windSpeed = currentWeather.data.instant.details.wind_speed;
            var windDirection = currentWeather.data.instant.details.wind_from_direction;

            // Optionally, get the symbol code for weather condition
            string? symbolCode = null;
            if (currentWeather.data.next_1_hours != null && currentWeather.data.next_1_hours.summary != null)
            {
                symbolCode = currentWeather.data.next_1_hours.summary.symbol_code;
            }

            var condition = SymbolCodeExtensions.ParseEnumByEnumMemberValue<SymbolCode>(symbolCode);
            ConditionLabel.Text = condition?.GetMeaning() ?? "Unknown weather condition";

            TemperatureLabel.Text = $"{temp}°C";

            // MessageLabel.Text = condition?.GetFunnyMessage() ?? "Unknown weather condition";

            WeatherIcon.Source = condition.ToString().ToLower() + ".png";
            WeatherIcon.IsVisible = true;

            WindLabel.Text = $"Wind {windSpeed} m/s"; //to do m/s

            WindArrow.Rotation = (double)windDirection; 

            //if (string.IsNullOrEmpty(WindArrow.AutomationId))
            //{
            //    WindArrow.AutomationId = $"Wind direction: {windDirection}°";
            //}

        }

        private async Task<Location?> GetLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                    return location;
                //// After getting the location:
                //var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);

                //if (placemarks != null)
                //{
                //    var placemark = placemarks.FirstOrDefault();
                //    if (placemark != null)
                //    {
                //        var town = placemark.Locality; // or placemark.SubAdminArea
                //        var country = placemark.CountryName;
                //        LocationLabel.Text = $"Location: {town}, {country} ({lat}, {lon})";
                //    }
                //    else
                //    {
                //        LocationLabel.Text = $"Location: {lat}, {lon}";
                //    }
                //}
                //else
                //{
                //    LocationLabel.Text = $"Location: {lat}, {lon}";
                //}
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "GPS not supported on this device.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Location permission denied.", "OK");
            }

            return null;
        }

        private static SKColor GetTemperatureColor(double temp)
        {
            return temp switch
            {
                <= -10 => SKColor.Parse("#0047AB"),
                <= 0 => SKColor.Parse("#007FFF"),
                <= 5 => SKColor.Parse("#00B3B3"),
                <= 10 => SKColor.Parse("#00CC66"),
                <= 15 => SKColor.Parse("#66CC00"),
                <= 20 => SKColor.Parse("#CCCC00"),
                <= 25 => SKColor.Parse("#FFCC00"),
                <= 30 => SKColor.Parse("#FF8C00"),
                <= 35 => SKColor.Parse("#FF4500"),
                _ => SKColor.Parse("#B22222")
            };
        }

        private void OnMinMaxDailyCanvasViewPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_maxDailyTemperatures.Count == 0 || _minDailyTemperatures.Count == 0 || _dailyDates.Count == 0)
                return;

            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;
            int margin = 50;
            int pointCount = _maxDailyTemperatures.Count;

            float minValue = Math.Min(_maxDailyTemperatures.Min(), _minDailyTemperatures.Min()) - 2;
            float maxValue = Math.Max(_maxDailyTemperatures.Max(), _minDailyTemperatures.Max()) + 2;

            float xStep = (width - 2 * margin) / (pointCount - 1);

            float GetY(float value) =>
                height - margin - ((value - minValue) / (maxValue - minValue)) * (height - 2 * margin);


            var valueLabelPaint = new SKPaint
            {
                Color = SKColors.DarkGray,
                IsAntialias = true
            };

            var font = new SKFont
            {
                Size = 24
            };

            DrawTemperatureCurve(canvas, _maxDailyTemperatures, _dailyDates, GetTemperatureColor, xStep, margin, height, GetY, font, valueLabelPaint, true);
            DrawTemperatureCurve(canvas, _minDailyTemperatures, _dailyDates, GetTemperatureColor, xStep, margin, height, GetY, font, valueLabelPaint, false);
        }

        private void DrawTemperatureCurve(
           SKCanvas canvas,
           IList<float> temperatures,
           IList<DateTime> dates,
           Func<double, SKColor> getColor, // Change delegate type to match GetTemperatureColor(double)  
           float xStep,
           int margin,
           int height,
           Func<float, float> GetY,
           SKFont font,
           SKPaint valueLabelPaint,
           bool isMaxCurve)
        {
            for (int i = 0; i < temperatures.Count - 1; i++)
            {
                float x1 = margin + i * xStep;
                float x2 = margin + (i + 1) * xStep;
                float y1 = GetY(temperatures[i]);
                float y2 = GetY(temperatures[i + 1]);

                var c1 = getColor(temperatures[i]);
                var c2 = getColor(temperatures[i + 1]);

                float ctrlX1 = x1 + xStep / 3;
                float ctrlX2 = x2 - xStep / 3;

                using (var path = new SKPath())
                {
                    path.MoveTo(x1, y1);
                    path.CubicTo(ctrlX1, y1, ctrlX2, y2, x2, y2);

                    var paint = new SKPaint
                    {
                        IsAntialias = true,
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 4,
                        Shader = SKShader.CreateLinearGradient(
                            new SKPoint(x1, y1),
                            new SKPoint(x2, y2),
                            new[] { c1, c2 },
                            null,
                            SKShaderTileMode.Clamp)
                    };

                    canvas.DrawPath(path, paint);
                }

                // Data point  
                canvas.DrawCircle(x1, y1, 6, new SKPaint { Color = c1, IsAntialias = true });

                // Label  
                float labelOffset = isMaxCurve ? -10 : 25;
                canvas.DrawText($"{temperatures[i]}°", x1, y1 + labelOffset, SKTextAlign.Center, font, valueLabelPaint);

                // Draw date labels only once (in one curve, e.g., max)  
                if (isMaxCurve)
                {
                    string dateLabel = dates[i].ToString("dd.MM");
                    canvas.DrawText(dateLabel, x1, height - 10, SKTextAlign.Center, font, valueLabelPaint);
                }

                // Last point  
                if (i == temperatures.Count - 2)
                {
                    canvas.DrawCircle(x2, y2, 6, new SKPaint { Color = c2, IsAntialias = true });
                    canvas.DrawText($"{temperatures[i + 1]}°", x2, y2 + labelOffset, SKTextAlign.Center, font, valueLabelPaint);

                    if (isMaxCurve)
                    {
                        string lastDateLabel = dates[i + 1].ToString("dd.MM");
                        canvas.DrawText(lastDateLabel, x2, height - 10, SKTextAlign.Center, font, valueLabelPaint);
                    }
                }
            }
        }

        private void DrawRainCurve(
            SKCanvas canvas,
            IList<float> values,
            List<DateTime> dates,
            Func<double, SKColor> getColor,
            float xStep,
            int margin,
            int height,
            Func<float, float> GetY,
            SKFont font,
            SKPaint valueLabelPaint,
            bool isMaxCurve)
        {
            float yBase = GetY(0); // Y position for 0 baseline

            for (int i = 0; i < values.Count - 1; i++)
            {
                float x1 = margin + i * xStep;
                float x2 = margin + (i + 1) * xStep;
                float y1 = GetY(values[i]);
                float y2 = GetY(values[i + 1]);

                var c1 = getColor(values[i]);
                var c2 = getColor(values[i + 1]);

                float ctrlX1 = x1 + xStep / 3;
                float ctrlX2 = x2 - xStep / 3;

                // Build the area path segment (trapezoid under Bezier curve)
                using (var areaPath = new SKPath())
                {
                    areaPath.MoveTo(x1, yBase);
                    areaPath.LineTo(x1, y1);
                    areaPath.CubicTo(ctrlX1, y1, ctrlX2, y2, x2, y2);
                    areaPath.LineTo(x2, yBase);
                    areaPath.Close();

                    var fillPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.LightSkyBlue.WithAlpha(80),
                        IsAntialias = true
                    };

                    canvas.DrawPath(areaPath, fillPaint);
                }

                // Draw the Bezier curve stroke over the filled area
                using (var path = new SKPath())
                {
                    path.MoveTo(x1, y1);
                    path.CubicTo(ctrlX1, y1, ctrlX2, y2, x2, y2);

                    var strokePaint = new SKPaint
                    {
                        IsAntialias = true,
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 2,
                        Shader = SKShader.CreateLinearGradient(
                            new SKPoint(x1, y1),
                            new SKPoint(x2, y2),
                            new[] { c1, c2 },
                            null,
                            SKShaderTileMode.Clamp)
                    };

                    canvas.DrawPath(path, strokePaint);
                }

                // Data point
                canvas.DrawCircle(x1, y1, 2, new SKPaint { Color = c1, IsAntialias = true });

                // Label
                float labelOffset = -10;
                canvas.DrawText($"{values[i]}mm", x1, y1 + labelOffset, SKTextAlign.Center, font, valueLabelPaint);

                if (i == values.Count - 2)
                {
                    canvas.DrawCircle(x2, y2, 6, new SKPaint { Color = c2, IsAntialias = true });
                    canvas.DrawText($"{values[i + 1]}mm", x2, y2 + labelOffset, SKTextAlign.Center, font, valueLabelPaint);
                }
            }
        }


        private void OnDailyRainCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_rainSummary.Count == 0)
                return;

            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;
            int margin = 50;
            int pointCount = _rainSummary.Count;

            float minValue = 0;
            float maxValue = _rainSummary.Max();

            float xStep = (width - 2 * margin) / (pointCount - 1);

            float GetY(float value) =>
                height - margin - ((value - minValue) / (maxValue - minValue)) * (height - 2 * margin);

            var valueLabelPaint = new SKPaint
            {
                Color = SKColors.DarkGray,
                IsAntialias = true
            };

            var font = new SKFont
            {
                Size = 24
            };
            DrawRainCurve(canvas, _rainSummary, _dailyDates, GetRainColor, xStep, margin, height, GetY, font, valueLabelPaint, false);
        }

        private static SKColor GetRainColor(double temp)
        {
            return temp switch
            {
                <= 0 => SKColor.Parse("#66666666"),
                _ => SKColors.DeepSkyBlue
            };
        }

    }
}
