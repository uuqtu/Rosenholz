using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Rosenholz.NotificationWindow
{
    internal static class Animations
    {
        //Animation Templates
        internal static DoubleAnimation FadeOut
        {
            get
            {
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.18))
                };
                return anim;
            }
        }

        internal static DoubleAnimation FadeIn
        {
            get
            {
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.18))
                };
                return anim;
            }
        }
    }
}
