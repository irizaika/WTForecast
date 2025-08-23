using System;
using SkiaSharp;

namespace WTForecast.Utils
{
    public static class DisplayUtils
    {
        public static SKColor GetTemperatureColor(double temp)
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

        public static SKColor GetRainColor(double temp)
        {
            return temp switch
            {
                <= 0 => SKColor.Parse("#66666666"),
                _ => SKColors.DeepSkyBlue
            };
        }
        public static SKColor Darken(SKColor color, float factor = 1.5f)
        {
            byte R(byte c) => (byte)Math.Clamp(c * factor, 0, 255);
            return new SKColor(R(color.Red), R(color.Green), R(color.Blue), color.Alpha);
        }


        public static void DrawRainCurve(
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

        public static void DrawTemperatureCurve(
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

    }
}
