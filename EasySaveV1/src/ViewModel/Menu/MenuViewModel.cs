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
            Refresh(); // construit les éléments initialement
        }

        public string CurrentLanguage => _localizer.GetCurrentLanguage();
        public string SelectLanguageLabel => _localizer["select_language"];

        public void Refresh()
        {
            Items = new List<MenuItem>
            {
                new MenuItem(_localizer["manage_jobs"],     () => _appController.RunManageJobs()),
                new MenuItem(_localizer["execute_backup"],  () => _appController.RunExecuteBackup()),
                new MenuItem(_localizer["exit"],            () => _appController.Exit())
            };
        }

        public void NavigateToSettings()
        {
            _appController.RunSettings();
        }

    }
}