using BackUp.ViewModel;
using System;

namespace BackUp.View
{
    public class MenuView : IView
    {
        private readonly MenuViewModel _menuVm;

        public MenuView(MenuViewModel menuVm)
        {
            _menuVm = menuVm;
        }

        public void Run()
        {
            int selected = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();

                // --- Header avec la langue actuelle ---
                Console.WriteLine($"=== EasySave v1.0.0 | Language: {_menuVm.CurrentLanguage} ===\n");
                Console.WriteLine($"     {_menuVm.SelectLanguageLabel}\n");
                // --- Menu principal ---
                for (int i = 0; i < _menuVm.Items.Count; i++)
                {
                    var item = _menuVm.Items[i];
                    if (i == selected) Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(item.Label);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && selected > 0)
                    selected--;
                else if (key == ConsoleKey.DownArrow && selected < _menuVm.Items.Count - 1)
                    selected++;

                else if (key == ConsoleKey.Enter)
                    _menuVm.Items[selected].Action();
                else if (key == ConsoleKey.Tab)
                    _menuVm.NavigateToSettings();
                    _menuVm.Refresh();

            } while (true);
        }
    }
}
