// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using View;
using System.IO;
using System.Text.Json;
using BackUp.ViewModel;
using Model;
using System.Resources;
using System.Transactions;

public class Program
{
    public static void Main()
    {
        Console.Write("================== TESTS ===================");
        TestTranslationManager.Run();
        /*
        Console.Write("Loading AppConfig ...");
        var configManager = new ConfigManager("./env/jobs.json", "./env/appconfig.json");
        var config = configManager.LoadAppConfig();
        CultureInfo.CurrentUICulture = new CultureInfo(config.Language);

        var services = new ServiceCollection();
        services.AddSingleton(new TranslationManager(config.Language));

        var provider = services.BuildServiceProvider();

        var translationManager = provider.GetRequiredService<TranslationManager>();
        ILocalizer localizer = new Localizer(translationManager);

        Run(localizer);
        var view = new MainView(localizer);
        view.Run();
    }
    public static void Run(ILocalizer localizer)
    {
        Console.WriteLine("Hello, World!");
        IView obj = new MainView(localizer);
        obj.Run();*/
    }
}


