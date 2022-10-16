using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Extensions
{
    internal static class Filler
    {
        internal static string GetFiller(int width)
        {
            return GetFiller(" ", width);
        }

        internal static string GetFiller(string txt, int width)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < width; i++) sb.Append(txt);
            return sb.ToString();
        }
    }
}
