using BackUp.ViewModel;
using System;
using BackUp.View;

namespace BackUp.View
{
    public class MainView : IView
    {
        private readonly ILocalizer _localizer;
        private readonly MainViewModel _viewModel;  // Injection du ViewModel
        private readonly LanguageSelectionView _languageSelectionView;
        private readonly ExecuteBackUpView _ExecuteBackUpView;  // Injection du ViewModel


        public MainView(ILocalizer localizer)
        {
            _localizer = localizer;
            _viewModel = new MainViewModel(_localizer);  // Injection du ViewModel
            _languageSelectionView = new LanguageSelectionView(_viewModel);
            _ExecuteBackUpView = new ExecuteBackUpView(_viewModel);

        }

        public void Run()
        {
            int selectedIndex = 0;  // Indice du menu principal
            bool _languageChanged = false; // Indique si l'on est sur "Language"

            // Initialisation des éléments du menu
            string[] menuItems = GetMenuItems();

            ConsoleKey key;
            do
            {
                Console.Clear();
                DisplayHeader();
                Console.WriteLine(_localizer["welcome_message"]);
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

                key = Console.ReadKey(true).Key;

                // Si l'utilisateur appuie sur Tab, on passe à la sélection de la langue
                if (key == ConsoleKey.Tab)
                {
                    _languageSelectionView.Run();
                    if (_languageSelectionView.GetLanguageChanged())
                    {
                        // Si la langue a changé, on met à jour les éléments du menu
                        DisplayHeader();
                        menuItems = GetMenuItems();
                    }
                }

                if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                    selectedIndex--;
                else if (key == ConsoleKey.DownArrow && selectedIndex < menuItems.Length - 1)
                    selectedIndex++;
                
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
                            _ExecuteBackUpView.Run();
                            break;
                        case 2:
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }

            } while (true);
        }

        private string[] GetMenuItems()
        {
            // Utilisation de MainViewModel pour obtenir les éléments du menu
            return new string[]
            {
                _viewModel.GetTranslation("manage_jobs"),
                _viewModel.GetTranslation("execute_backup"),
                _viewModel.GetTranslation("exit")
            };
        }


        private void DisplayHeader()
        {
            string version = "1.0.0"; // Version à personnaliser
            string languageText = $"Language: {_viewModel.GetCurrentLanguage()}";

            Console.WriteLine($"================ EasySave ================ Version: {version} | {languageText}");

            // Affichage du "Language" comme sélectionnable si on y est

            Console.WriteLine($"  {_viewModel.GetTranslation("select_language")}");
        }
    }
}
