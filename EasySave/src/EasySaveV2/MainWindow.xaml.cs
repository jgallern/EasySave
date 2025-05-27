using Core.ViewModel;
using Core.Model.Services;
using Core.ViewModel.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Core.ViewModel.Notifiers;
using EasySaveV2.Notifiers;

namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IUIErrorNotifier notifier = new MessageBoxNotifier();
            INavigationService navigation = new NavigationService();
            MainViewModel SettingsVM = new MainViewModel(navigation, notifier);
            DataContext = SettingsVM;
        }
    }
}