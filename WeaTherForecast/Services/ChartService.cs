using Microcharts;
using SkiaSharp;

namespace WTForecast.Services
{
    public class ChartService
    {
        public Chart CreateHourlyTemperatureChart(List<ChartEntry> entries, double minTemp, double maxTemp)
        {
            return new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                LineSize = 3,
                PointSize = 8,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelTextSize = 20,
                MinValue = (float)minTemp - 2,
                MaxValue = (float)maxTemp + 2,
                Margin = 5,
                LineMode = LineMode.Straight
            };
        }

        public Chart CreateHourlyRainChart(List<ChartEntry> entries, double maxRain)
        {
            return new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                LineSize = 2,
                PointSize = 2,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColors.White,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 10,
                MinValue = 0,
                MaxValue = (float)maxRain,
                Margin = 0,
                LineMode = LineMode.Spline
            };
        }
    }

}
