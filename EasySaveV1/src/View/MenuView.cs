using BackUp.ViewModel;
using System;
using BackUp.View;

public class MenuView : IView
{
    private readonly AppController _appController;
    private readonly List<MenuItem> _items;

    public MenuView(AppController controller)
    {
        _appController = controller;
        _items = BuildMenu();
    }

    private List<MenuItem> BuildMenu()
    {
        return new List<MenuItem>
        {
            new MenuItem(_appController.Translate("manage_jobs"),     () => _appController.RedirectManageBackupsCommand.Execute(null)),
            new MenuItem(_appController.Translate("execute_backup"),  () => _appController.RedirectExecuteBackupCommand.Execute(null)),
            new MenuItem(_appController.Translate("exit"),            () => _appController.ExitCommand.Execute(null))
        };
    }

    public void Run()
    {
        int selected = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine($"=== EasySave v1.0.0 | Language: {_appController.GetCurrentLanguage()} ===\n");
            Console.WriteLine($"        {_appController.Translate("select_language")}\n\n");
            for (int i = 0; i < _items.Count; i++)
            {
                if (i == selected)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(_items[i].Label);
                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && selected > 0) selected--;
            else if (key == ConsoleKey.DownArrow && selected < _items.Count - 1) selected++;
            else if (key == ConsoleKey.Enter) _items[selected].Action();
            else if (key == ConsoleKey.Tab)
            {
                _appController.RedirectSettingsCommand.Execute(null);
                _items.Clear();
                _items.AddRange(BuildMenu());
            }

        } while (true);
    }
}
