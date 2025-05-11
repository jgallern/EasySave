// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.IO;
using System.Text.Json;
using BackUp;
using BackUp.View;
using BackUp.ViewModel;
using System.Resources;
using System.Transactions;

public class Program
{
    public static void Main()
    {
        Console.Write("Loading AppConfig language...\n");

        ILocalizer localizer = Localizer.Instance;
        CultureInfo.CurrentUICulture = new CultureInfo(localizer.GetCurrentLanguage());

        // Exécution de la vue principale
        Run(localizer);
    }
    public static void Run(ILocalizer localizer)
    {
        Console.WriteLine("Hello, World!");
        IView app = new MainView(localizer);
        app.Run();
    }


    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
    TestTranslationManager.Run();
    */
}

