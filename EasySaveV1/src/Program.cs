// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Resources;
using View;

using System.IO;
using System.Text.Json;
using ViewModel;

public class Program
{
    public static void Main()
    {
        Console.Write("Loading AppConfig ...");
        var config = ConfigurationService.Load();
        CultureInfo.CurrentUICulture = new CultureInfo(config.Language);

        var services = new ServiceCollection();
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var provider = services.BuildServiceProvider();

        var stringLocalizer = provider.GetRequiredService<IStringLocalizer<Resources.Strings>>();
        ILocalizer localizer = new ViewModel.Localizer(stringLocalizer);

        Run(localizer);
        var view = new MainView(localizer);
        view.Run();
    }
    public static void Run(ILocalizer localizer)
    {
        Console.WriteLine("Hello, World!");
        IView obj = new MainView(localizer);
        obj.Run();
    }
}


public class AppConfig
{
    public string Language { get; set; } = "en";
}

public static class ConfigurationService
{
    public static AppConfig Load()
    {
        var json = File.ReadAllText("AppConfig.json");
        return JsonSerializer.Deserialize<AppConfig>(json);
    }
}



