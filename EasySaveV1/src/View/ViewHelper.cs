using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackUp.ViewModel;

namespace BackUp.View
{
    public class ViewHelper
    {
       
       private readonly MainViewModel _viewModel;

       public ViewHelper(MainViewModel viewModel)
       {
           _viewModel = viewModel;
       }

        public void DisplayHeader(bool isLanguageSelected = false)
        {
            string version = "1.0.0"; // Version à personnaliser
            string languageText = $"Language: {_viewModel.GetCurrentLanguage()}";

            Console.WriteLine($"================ EasySave ================ Version: {version} | {languageText}");

            // Affichage du "Language" comme sélectionnable si on y est
            if (isLanguageSelected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("> [Language] (Press Enter to Change)");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("  Language: (Use Tab to change language)");
            }
        }


        public bool DisplayLanguageSelection()
        {
            List<string> availableLanguages = _viewModel.GetAvailableLanguages();
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Select a Language ===\n");
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

                // Validation de la langue avec Enter
                if (key == ConsoleKey.Enter)
                {
                    string selectedLanguage = availableLanguages[selectedIndex];

                    if (selectedLanguage != _viewModel.GetCurrentLanguage())
                    {
                        _viewModel.ChangeLanguage(selectedLanguage);  // Changer la langue
                        return true;  // Indiquer que la langue a été changée
                    }
                    return false;  // Si la langue est déjà sélectionnée, ne rien faire
                }

                // Permet de revenir au menu principal avec Tab
                if (key == ConsoleKey.Tab)
                {
                    return false;  // Ne pas changer la langue et revenir au menu principal
                }

            } while (key != ConsoleKey.Enter && key != ConsoleKey.Tab);  // Continue jusqu'à ce que Enter ou Tab soit pressé

            return false;  // Si la boucle se termine sans valider la langue, retourner false par défaut
        }



    }
}
