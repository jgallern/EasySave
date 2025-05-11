using BackUp.ViewModel;
using System;
using BackUp.View;

namespace BackUp.View
{
    public class LanguageSelectionView : IView
    {
        private readonly MainViewModel _viewModel;
        private bool _languageChanged;

        public LanguageSelectionView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Run()
        {
            _languageChanged = false;
            List<string> availableLanguages = _viewModel.GetAvailableLanguages();
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine($"=== {_viewModel.GetTranslation("language_menu")} ===\n");

                for (int i = 0; i < availableLanguages.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {availableLanguages[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {availableLanguages[i]}");
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                    selectedIndex--;
                else if (key == ConsoleKey.DownArrow && selectedIndex < availableLanguages.Count - 1)
                    selectedIndex++;

                if (key == ConsoleKey.Enter)
                {
                    string selectedLanguage = availableLanguages[selectedIndex];
                    if (selectedLanguage != _viewModel.GetCurrentLanguage())
                    {
                        _viewModel.ChangeLanguageCommand.Execute(selectedLanguage);
                        _languageChanged = true;
                    }
                    break;
                }

                if (key == ConsoleKey.Tab)
                {
                    break;
                }

            } while (true);
        }

        public bool GetLanguageChanged()
        {
            return _languageChanged;
        }
    }
}
