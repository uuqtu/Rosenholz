using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.TextTables
{
    internal static class TextEncoder
    {
        internal static string EscapeXml(string txt)
        {
            if (txt == null) return null;
            var sb = new StringBuilder();
            for (int i = 0; i < txt.Length; i++)
            {
                var c = txt[i];
                if (c < 32 || c > 127 || c == '<' || c == '>' || c == '&' || c == '\''
                    || c == '\"')
                {
                    sb.Append("&#" + (int)c + ";");
                }
                else sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
