﻿using HomeSpeaker.MAUI.Services;
using HomeSpeaker.MAUI.ViewModels;
using HomeSpeaker.MAUI.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
namespace HomeSpeaker.MAUI;

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

        builder.Services.AddSingleton<IHomeSpeakerMauiService>(provider =>
        {
            string baseUrl = "http://localhost:5280";
            return new HomeSpeakerMauiService(baseUrl);
        });

        builder.Services.AddSingleton<MusicViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MusicPage>();
        builder.Services.AddSingleton<ServerSettingsViewModel>();
        builder.Services.AddSingleton<ServerSettingsPage>();
        builder.Services.AddSingleton<YouTubeService>();
        builder.Services.AddSingleton<YouTubeViewModel>();
        builder.Services.AddSingleton<YouTubePage>();


        //AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        //{
        //    Debug.WriteLine($"[ERROR] Unhandled Exception: {e.ExceptionObject}");
        //};


        //builder.Logging.AddDebug();
        //builder.Logging.SetMinimumLevel(LogLevel.Trace);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
