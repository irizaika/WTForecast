using SkiaSharp;
using WTForecast.Models;
using WTForecast.Utils;

namespace WTForecast.Services
{
    public class DailyChartDrawer
    {
        public void DrawTemperatureRange(SKCanvas canvas, SKImageInfo info, List<DailyTemperatureRangeData> dailyTemperatureRangeData)
        {
            if (dailyTemperatureRangeData.Count == 0)
                return;

            canvas.Clear(SKColors.Transparent);

            int width = info.Width;
            int height = info.Height;
            int margin = 50;
            int pointCount = dailyTemperatureRangeData.Count;

            float minValue = (float)Math.Min(dailyTemperatureRangeData.Select(t => t.maxTemperature).Min(), dailyTemperatureRangeData.Select(t => t.minTemperature).Min()) - 2;
            float maxValue = (float)Math.Max(dailyTemperatureRangeData.Select(t => t.maxTemperature).Max(), dailyTemperatureRangeData.Select(t => t.minTemperature).Max()) + 2;

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

            var maxDailyTemperatures = dailyTemperatureRangeData.Select(max => max.maxTemperature).ToList();
            var minDailyTemperatures = dailyTemperatureRangeData.Select(max => max.minTemperature).ToList();
            var dailyDates = dailyTemperatureRangeData.Select(max => max.date).ToList();

            DisplayUtils.DrawTemperatureCurve(canvas, maxDailyTemperatures, dailyDates, DisplayUtils.GetTemperatureColor, xStep, margin, height, GetY, font, valueLabelPaint, true);
            DisplayUtils.DrawTemperatureCurve(canvas, minDailyTemperatures, dailyDates, DisplayUtils.GetTemperatureColor, xStep, margin, height, GetY, font, valueLabelPaint, false);
        }

        public void DrawRainAmount(SKCanvas canvas, SKImageInfo info, List<DailyRainAmountData> dailyRainAmountData)
        {
            if (dailyRainAmountData.Count == 0)
                return;

            canvas.Clear(SKColors.Transparent);

            int width = info.Width;
            int height = info.Height;
            int margin = 50;
            int pointCount = dailyRainAmountData.Count;

            float minValue = 0;
            float maxValue = dailyRainAmountData.Select(r => r.sumRain).Max();

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
            DisplayUtils.DrawRainCurve(canvas, dailyRainAmountData.Select(r => r.sumRain).ToList(), dailyRainAmountData.Select(r => r.date).ToList(), DisplayUtils.GetRainColor, xStep, margin, height, GetY, font, valueLabelPaint, false);
        }

    }
}
