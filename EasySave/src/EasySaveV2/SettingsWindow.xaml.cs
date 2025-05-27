using Core.Model.Services;
using Core.ViewModel.Services;
using Core.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            INavigationService _navigation = new NavigationService();
            SettingsViewModel SettingsVM = new SettingsViewModel(_navigation);
            DataContext = SettingsVM;
        }
    }
}