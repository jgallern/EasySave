// See https://aka.ms/new-console-template for more information

using System;
using System.Globalization;
using BackUp;
using BackUp.ViewModel;
using BackUp.View;

public class Program
{
    public static void Main()
    {
        Console.Write("Loading AppConfig language...\n");

        ILocalizer localizer = Localizer.Instance;
        CultureInfo.CurrentUICulture = new CultureInfo(localizer.GetCurrentLanguage());

        // Création du contrôleur principal
        IAppController appController = new AppController(localizer);

        // Lancement de l'application
        appController.RunApp();
    }


    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
    TestTranslationManager.Run();
    */
}

