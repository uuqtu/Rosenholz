using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.NotificationWindow
{
    public static class NotificationWindowShower
    {
        public static NotificationWindow Notification
        {
            get => _notification;
            set
            {
                if (value == null)
                {
                    _notification = null;
                }
                else
                {
                    _notification?.Close();
                    _notification = value;
                }
            }
        }

        private static NotificationWindow _notification;

        public static void ShowNotification(string text, NotificationWindow.NotificationType type, bool autoHide, Action onClick = null)
        {
            Notification = new NotificationWindow(text, type, autoHide, onClick);
            Notification.Show();
        }

        public static async Task ShowNotificationAsync(string text, NotificationWindow.NotificationType type, Action onClick = null)
        {
            Notification = new NotificationWindow(text, type, true, onClick);
            await Notification.ShowAsync();
        }
    }
}
