using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTutorialStep1
{
    public partial class MainWindow : Window
    {
        private string _filePath = "";
        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                _filePath = args[1];
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm vmo = new vm(_filePath);
            this.DataContext = vmo;
        }
    }
}
