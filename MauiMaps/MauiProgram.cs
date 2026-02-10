namespace MauiMaps;

using AutoFixture;
using AutoFixture.Kernel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Maps;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			   .UseMauiCommunityToolkit()
			   .UseMauiMaps()
			   .UseMauiCommunityToolkitMaps("YOUR_KEY") // https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
			   .UsePrism(prism =>
			   {
				   prism.RegisterTypes(container =>
				   {
					   var fixture = new Fixture();
				   fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
					   .ForEach(b => fixture.Behaviors.Remove(b));
				   fixture.Behaviors.Add(new OmitOnRecursionBehavior());
				   container.RegisterInstance<IFixture>(fixture);
					   container.RegisterForNavigation<MainPage, MainPageViewModel>();
					   container.RegisterForNavigation<SecondPage, SecondPageViewModel>();
				   });
				   prism.CreateWindow("/MainPage");
			   });
		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
		});

		return builder.Build();
	}
}
