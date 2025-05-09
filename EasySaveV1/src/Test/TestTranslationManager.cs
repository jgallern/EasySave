using BackUp.ViewModel;

public class TestTranslationManager
{
    public static void Run()
    {
        Console.WriteLine("Test TranslationManager :");
        var manager = new TranslationManager();

        Console.WriteLine("Test GetTranslation :");
        Console.WriteLine(manager.GetTranslation("welcome_message"));
        Console.WriteLine(manager.GetTranslation("manage_jobs"));

        // Test d'autres méthodes
        manager.ChangeLanguage("fr");
        Console.WriteLine(manager.GetTranslation("welcome_message"));

    }
}