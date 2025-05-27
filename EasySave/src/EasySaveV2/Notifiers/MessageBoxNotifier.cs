using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core.ViewModel.Notifiers;

namespace EasySaveV2.Notifiers
{
    public class MessageBoxNotifier : IUIErrorNotifier
    {
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public bool ShowWarning(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return result == MessageBoxResult.OK;
        }
    }
}
