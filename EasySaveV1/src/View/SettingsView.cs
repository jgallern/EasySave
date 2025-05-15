using BackUp.ViewModel;
using System;
using BackUp.View;

namespace BackUp.View
{
    public class SettingsView : IView
    {
        private readonly ISettingsViewModel _vm;
        private readonly IAppController _app;
        private bool _languageChanged;

        public SettingsView(IAppController app, ISettingsViewModel vm)
        {
            _vm = vm;
            _app = app;
        }

        public void Run()
        {
            _languageChanged = false;
            var langs = _app.GetAvailableLanguages();
            int idx = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine($"=== {_app.Translate("language_menu")} ===\n");
                for (int i = 0; i < langs.Count; i++)
                {
                    if (i == idx) Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(langs[i]);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && idx > 0) idx--;
                else if (key == ConsoleKey.DownArrow && idx < langs.Count - 1) idx++;

                if (key == ConsoleKey.Enter)
                {
                    var selected = langs[idx];
                    if (selected != _vm.CurrentLanguage)
                    {
                        _vm.ChangeLanguageCommand.Execute(selected);
                        _languageChanged = true;
                    }
                    break;
                }
                if (key == ConsoleKey.Tab) break;
                if (key == ConsoleKey.Escape) break;

            } while (true);
        }

        public bool LanguageChanged => _languageChanged;
    }
}

