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
    }
}
