using BackUp.ViewModel;

public class TestTranslationManager
{
    public static void Run()
    {
        Console.WriteLine("Test TranslationManager :");
        TranslationManager manager = new TranslationManager();

        Console.WriteLine("Test GetTranslation :");
        Console.WriteLine(manager.GetTranslation("welcome_message"));
        Console.WriteLine(manager.GetTranslation("manage_jobs"));

        // Test si le passage a une autre langue est bon
        manager.ChangeLanguage("fr");
        Console.WriteLine(manager.GetTranslation("welcome_message"));

        // Test la langue par défault quand il existe pas 
        manager.ChangeLanguage("ar");
        Console.WriteLine(manager.GetTranslation("welcome_message"));

    }
}