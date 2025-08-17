using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Rosenholz.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        #region Constants and Fields
        /////https://stackoverflow.com/questions/14506406/wpf-single-instance-best-practices
        /////
        ///// <summary>The event mutex name.</summary>
        //private const string UniqueEventName = "{88974gfvdfgsdfhgsdhbgfdsfgdfgsdfgdsfgsydengffdxvchgxb1a}";

        ///// <summary>The unique mutex name.</summary>
        //private const string UniqueMutexName = "{88974gfvdfgsdfhgsdhbgfdsfgdfgsdfgdsfgsydengffdxvchgxb1a}";

        ///// <summary>The event wait handle.</summary>
        //private EventWaitHandle eventWaitHandle;

        ///// <summary>The mutex.</summary>
        //private Mutex mutex;

        #endregion

        private static Mutex _mutex = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //#warning new code for only one instance could break sth
            //            const string appName = "MyAppName";
            //            bool createdNew;

            //            _mutex = new Mutex(true, appName, out createdNew);

            //            if (!createdNew)
            //            {
            //                //app is already running! Exiting the application
            //                //Application.Current.Shutdown();
            //                this.Shutdown();
            //            }

            //            base.OnStartup(e);

            //bool isOwned;
            //this.mutex = new Mutex(true, UniqueMutexName, out isOwned);
            //this.eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);

            //// So, R# would not give a warning that this variable is not used.
            //GC.KeepAlive(this.mutex);

            //if (isOwned)
            //{
            //    // Spawn a thread which will be waiting for our event
            //    var thread = new Thread(
            //        () =>
            //        {
            //            while (this.eventWaitHandle.WaitOne())
            //            {
            //                Current.Dispatcher.BeginInvoke(
            //                    (Action)(() => ((MainWindow)Current.MainWindow).BringToForeground()));
            //            }
            //        });

            //    // It is important mark it as background otherwise it will prevent app from exiting.
            //    thread.IsBackground = true;

            //    thread.Start();
            //    return;
            //}

            //// Notify other instance so it could bring itself to foreground.
            //this.eventWaitHandle.Set();

            //// Terminate this instance.
            //this.Shutdown();

#warning Singleton implementierung etwas schlecht.

#if !DEBUG

            var allProcesses = Process.GetProcesses();

            var possibleProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(possibleProcessName);

            if (processes?.Count() > 1)
            {
                MessageBox.Show("Already running!");
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                InitialSettings settingsWindow = new InitialSettings();
                MainWindow mainWindow = new MainWindow();

                settingsWindow.ShowDialog();

                if (settingsWindow.DialogResult == true)
                {
                    mainWindow.Show();
                }
                else
                {
                    mainWindow.IsDesiredCloseButtonClicked = true;
                    mainWindow.AskForValidation = false;
                    mainWindow.Close();
                }
            }
#else
            InitialSettings settingsWindow = new InitialSettings();
            MainWindow mainWindow = new MainWindow();

            settingsWindow.ShowDialog();

            if (settingsWindow.DialogResult == true)
            {
                mainWindow.Show();
            }
            else
            {
                mainWindow.IsDesiredCloseButtonClicked = true;
                mainWindow.AskForValidation = false;
                mainWindow.Close();
            }
#endif

        }
    }
}
