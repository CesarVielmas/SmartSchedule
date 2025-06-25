using Microsoft.Extensions.Logging;
using ProjectTSSI.Global;

namespace ProjectTSSI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
		{
			// Maneja la excepción no controlada 
			LogUnhandledException((Exception)e.ExceptionObject);
		};
#if WINDOWS
		Logger.Init();
		MainPage = new NavigationPage(new LoginWS());
		// #elif ANDROID 
		// 	MainPage = new NavigationPage(new AndroidPage());
		// #elif IOS
		// 	MainPage = new NavigationPage(new iOSPage());
		// #elif TIZEN
		// 	MainPage = new NavigationPage(new TizenPage());
#else
        	MainPage = new NavigationPage(new MainPage());
#endif
	}
	private void LogUnhandledException(Exception ex)
	{
		try
		{
			var logger = LoggerFactory.Create(builder =>
			{
				builder.AddConsole();
			}).CreateLogger<App>(); logger.LogError(ex, "Unhandled exception occurred.");
		}
		catch
		{

		}
	}
}
