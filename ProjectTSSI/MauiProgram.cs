using Microsoft.Extensions.Logging;
using FFImageLoading.Maui;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Windows.System;
using WinRT;
using Microsoft.UI.Composition;
using ProjectTSSI.Global;
using Windows.UI.Core;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Services;
namespace ProjectTSSI;

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
                fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
                fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                fonts.AddFont("Poppins-Light.ttf", "Poppins");
            });

#if WINDOWS
        builder.Services.AddSingleton<IAnimationsService, TextAnimationService>();
        builder.Services.AddSingleton<IAnimationsService, ContentAnimationService>();
        builder.Services.AddSingleton<IAnimationServiceComposer,ComposerAnimationsService>();
        builder.Services.AddTransient<RegisterWS>();
        builder.Logging.ClearProviders();
        builder.UseFFImageLoading();
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
        builder.Services.AddLogging(configure =>
        {
            configure.AddDebug();
        });
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(Entry), typeof(CustomEntryHandler));
        });
        builder.ConfigureLifecycleEvents(lifecycle =>
        {
            lifecycle.AddWindows(windows =>
            {
                windows.OnWindowCreated(window =>
                {
                    var realDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
                    var hwnd = WindowNative.GetWindowHandle(window);
                    var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                    var appWindow = AppWindow.GetFromWindowId(windowId);
                    var displayInfo = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
                    appWindow.Move(new Windows.Graphics.PointInt32((int)(displayInfo.WorkArea.Width * 0.06), (int)(displayInfo.WorkArea.Height * 0.15)));
                    appWindow.Resize(new Windows.Graphics.SizeInt32((int)(displayInfo.WorkArea.Width * 0.9), (int)(displayInfo.WorkArea.Height * 0.7)));
                    appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
                    appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
                    WindowExtensionsWindow.SetRoundedCorners(hwnd);
                    if (appWindow.Presenter is OverlappedPresenter presenter)
                    {
                        presenter.IsMaximizable = false;
                        presenter.IsMinimizable = false;
                        presenter.IsResizable = false;
                        presenter.SetBorderAndTitleBar(false, false);
                    }
                    if (window.Content is Microsoft.UI.Xaml.Controls.Panel panel)
                        panel.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
                });
            });
        });
#endif
        builder.Logging.AddDebug();
        builder.Logging.AddConsole();
        var app = builder.Build();
        GlobalConstants.ServiceProvider = app.Services;
        return app;
    }
}
