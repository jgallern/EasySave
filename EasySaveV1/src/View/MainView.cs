using View;
using BackUp.ViewModel;
using Microsoft.Extensions.Localization;
using BackUp.View;

public class MainView : IView
{
    private readonly ILocalizer _localizer;
    private readonly ViewHelper _viewhelper;
    private readonly MainViewModel _viewModel;

    public MainView(MainViewModel viewModel, ILocalizer localizer)
    {
        _localizer = localizer;
        _viewModel = viewModel;
        _viewhelper = new ViewHelper(viewModel); // Instanciation du helper

    }

    public void Run()
    {
        int selectedIndex = 0;  // Indice du menu principal
        bool isLanguageSelected = false; // Indique si l'on est sur "Language"

        // Initialisation des éléments du menu
        string[] menuItems = GetMenuItems();

        ConsoleKey key;
        do
        {
            Console.Clear();
            _viewhelper.DisplayHeader(isLanguageSelected); // Passer un bool pour savoir si on est sur "Language"
            Console.WriteLine(_viewModel.GetTranslation("welcome_message"));
            Console.WriteLine();

            // Affichage du menu
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {menuItems[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuItems[i]}");
                }
            }

            // Si on est sur "Language", on permet de naviguer et changer la langue
            if (isLanguageSelected)
            {
                _viewhelper.DisplayLanguageSelection();
            }

            key = Console.ReadKey(true).Key;

            // Si on est sur "Language" et que l'utilisateur appuie sur les flèches, on change la langue
            if (isLanguageSelected)
            {
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                {
                    _viewhelper.DisplayLanguageSelection();
                }
                else if (key == ConsoleKey.Enter)
                {
                    isLanguageSelected = false;  // Revenir au menu principal après avoir changé la langue
                    menuItems = GetMenuItems(); // Mettre à jour les éléments du menu
                }
                else if (key == ConsoleKey.Tab) // Si l'utilisateur appuie sur Tab, on revient au menu principal
                {
                    isLanguageSelected = false;
                }
            }
            else
            {
                if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                    selectedIndex--;
                else if (key == ConsoleKey.DownArrow && selectedIndex < menuItems.Length - 1)
                    selectedIndex++;

                // Si on est sur "Language" dans le header, on active le changement de langue avec Tab
                if (key == ConsoleKey.Tab)
                {
                    isLanguageSelected = true; // Passer à la sélection de langue
                }

                if (key == ConsoleKey.Enter)
                {
                    // Actions selon le menu sélectionné
                    switch (selectedIndex)
                    {
                        case 0:
                            Console.WriteLine("case 1 - manage jobs");
                            Console.ReadKey();
                            break;
                        case 1:
                            Console.WriteLine("case 2 - execute backup");
                            Console.ReadKey();
                            break;
                        case 2:
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
            }

        } while (true);
    }

    private string[] GetMenuItems()
    {
        return new string[]
        {
            _viewModel.GetTranslation("manage_jobs"),
            _viewModel.GetTranslation("execute_backup"),
            _viewModel.GetTranslation("exit")
        };
    }


}
