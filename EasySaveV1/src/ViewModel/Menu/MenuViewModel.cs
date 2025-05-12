using System;
using System.Collections.Generic;

namespace BackUp.ViewModel
{
    public class MenuViewModel : IMenuViewModel
    {
        private readonly IAppController _appController;
        private readonly ILocalizer _localizer;

        public IReadOnlyList<MenuItem> Items { get; private set; }

        public MenuViewModel(IAppController controller, ILocalizer localizer)
        {
            _appController = controller;
            _localizer = localizer;
            RefreshMenu(); // construit les éléments initialement
        }

        public string CurrentLanguage => _localizer.GetCurrentLanguage();
        public string SelectLabel(string id)
        {
            return _localizer[id];
        }

        public void RefreshMenu()
        {
            Items = new List<MenuItem>
            {
                new MenuItem(SelectLabel("manage_jobs"),     () => _appController.RunManageJobs()),
                new MenuItem(SelectLabel("execute_backup"),  () => _appController.RunExecuteBackup()),
                new MenuItem(SelectLabel("exit"),            () => _appController.Exit())
            };
        }

        public void NavigateToSettings()
        {
            _appController.RunSettings();
        }
        public void ExitApp(){
            _appController.Exit();
        }

    }
}