//using Core.ViewModel.Services;
using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Windows;

namespace Core.ViewModel.Services
{
    public class FileDialogService : IFileDialogService
    {

        public string SelectFolder()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Sélectionnez un dossier",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true
            };

            bool? result = dialog.ShowDialog();

            return result == true ? dialog.SelectedPath : null;
        }

    }
}
