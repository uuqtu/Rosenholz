using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
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


            InitialSettings settingsWindow = new InitialSettings();
            MainWindow mainWindow = new MainWindow();

            settingsWindow.ShowDialog();

            if (settingsWindow.DialogResult == true)
            { mainWindow.Show(); }
            else
            {
                mainWindow.IsDesiredCloseButtonClicked = true;
                mainWindow.AskForValidation = false;
                mainWindow.Close();
            }
        }
    }
}
