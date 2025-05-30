using Core.Model.Services;
using Core.ViewModel;
using Core.ViewModel.Notifiers;
using Core.ViewModel.Services;
using EasySaveV3.Notifiers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasySaveV3
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