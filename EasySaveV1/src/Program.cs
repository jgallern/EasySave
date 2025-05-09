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
        Console.Write("Loading AppConfig language...\n");

        // Création du TranslationManager pour lire la langue dans appconfig.json
        var translationManager = new TranslationManager();

        // Définir la culture de l'application en fonction du fichier appconfig
        string language = translationManager.GetCurrentLanguage();
        CultureInfo.CurrentUICulture = new CultureInfo(language);
        CultureInfo.CurrentCulture = new CultureInfo(language);
    /*
    // Configuration du conteneur d'injection de dépendances
    var services = new ServiceCollection();
    services.AddSingleton(translationManager);
    services.AddSingleton<ILocalizer, Localizer>();
    services.AddSingleton<MainViewModel>();
    services.AddSingleton<MainView>();

    var provider = services.BuildServiceProvider();

    // Résolution des dépendances
    var view = provider.GetRequiredService<MainView>();
    */
    MainViewModel viewModel = new MainViewModel(translationManager);
    ILocalizer localizer = new Localizer(translationManager);
        // Exécution de la vue principale
        Run(viewModel, localizer);
    }
    public static void Run(MainViewModel viewModel, ILocalizer localizer)
    {
        Console.WriteLine("Hello, World!");
        IView obj = new MainView(viewModel, localizer);
        obj.Run();
    }


    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
    TestTranslationManager.Run();
    */
}



