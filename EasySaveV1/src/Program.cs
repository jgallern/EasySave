// See https://aka.ms/new-console-template for more information

using System;
using System.Globalization;
using BackUp;
using BackUp.ViewModel;
using BackUp.View;
using BackUp.Model;

public class Program
{
    public static void Main()
    {        
        Console.Write("Loading AppConfig language...\n");

        ConfigManager.Initialize(@"c:\users\Florian\test.json");

        ILocalizer localizer = Localizer.Instance;
        CultureInfo.CurrentUICulture = new CultureInfo(localizer.GetCurrentLanguage());

        // Création du contrôleur principal
        IAppController appController = new AppController(localizer);
        // Lancement de l'application
        appController.RedirectMenuCommand.Execute(null);
    }


    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
    TestTranslationManager.Run();
    */
    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
        TestLogger.Run();
    */
    /* //---------------------- TESTS -------------------------
    Console.Write("================== TESTS ===================");
        TestBackUpFull.Run();
    */

}

