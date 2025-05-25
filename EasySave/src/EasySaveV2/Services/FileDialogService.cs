using Core.ViewModel.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

public class FileDialogService : IFileDialogService
{
    public string OpenFile(string filter = "All files (*.*)|*.*")
    {
        var dialog = new OpenFileDialog
        {
            Filter = filter,
            Multiselect = false
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string SaveFile(string filter = "All files (*.*)|*.*")
    {
        var dialog = new SaveFileDialog
        {
            Filter = filter
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    // Astuce : utiliser OpenFileDialog pour sélectionner un dossier
    public string SelectFolder()
    {
        var dialog = new OpenFileDialog
        {
            CheckFileExists = false,
            FileName = "Select this folder"
        };

        if (dialog.ShowDialog() == true)
        {
            return Path.GetDirectoryName(dialog.FileName);
        }

        return null;
    }
}
