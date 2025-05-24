using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;

namespace ProjectTSSI;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
	public override void OnCreate(){
            base.OnCreate();

            // Configurar la página principal de la aplicación
            MainPage = new NavigationPage(new EjemploPagina());
	}
}
