using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitialSettings settingsWindow = new InitialSettings();
            MainWindow mainWindow = new MainWindow();

            settingsWindow.ShowDialog();

            if (settingsWindow.DialogResult == true)
            { mainWindow.Show(); }
            else
            { mainWindow.Close(); }
        }
    }
}
