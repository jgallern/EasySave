﻿using Core.ViewModel;
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
using EasySaveV3.Notifiers;
using System.Windows;

namespace EasySaveV3
{
    /// <summary>
    /// Interaction logic for MonitoringWindow.xaml
    /// </summary>
    public partial class MonitoringWindow : Window
    {
        public MonitoringWindow(MonitoringViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}