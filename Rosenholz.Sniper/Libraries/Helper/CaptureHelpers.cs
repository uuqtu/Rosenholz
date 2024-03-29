﻿using Rosenholz.Sniper.Libraries.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Sniper.Libraries.Helper
{
    internal static class CaptureHelpers
    {

        internal static Point ScreenToClient(Point p)
        {
            int screenX = NativeMethods.GetSystemMetrics(SystemMetric.SmXvirtualscreen);
            int screenY = NativeMethods.GetSystemMetrics(SystemMetric.SmYvirtualscreen);
            return new Point(p.X - screenX, p.Y - screenY);
        }


        internal enum SystemMetric
        {
            /// <summary>
            /// Windows 2000 (v5.0+) Coordinate of the top of the virtual screen
            /// </summary>
            SmXvirtualscreen = 76,
            /// <summary>
            /// Windows 2000 (v5.0+) Coordinate of the left of the virtual screen
            /// </summary>
            SmYvirtualscreen = 77
        }
    }
}
