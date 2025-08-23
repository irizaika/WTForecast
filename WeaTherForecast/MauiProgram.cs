using Microcharts.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WTForecast.Services;
using WTForecast.Utils;

namespace WTForecast;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.UseSkiaSharp();

		// Register the ChartView handler
		//builder.ConfigureMauiHandlers(handlers =>
		//{
		//	handlers.AddHandler<ChartView, Cha>();
		//});

		builder.Services.AddSingleton<WeatherService>();
		builder.Services.AddSingleton<LocationService>();
		builder.Services.AddSingleton<WeatherMapper>();
		builder.Services.AddSingleton<ChartService>();
		builder.Services.AddSingleton<DailyChartDrawer>();
		builder.Services.AddSingleton<WeatherFacade>();

        return builder.Build();
	}
}
