using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms.Samples
{
    public static class StringExtensions
    {
        public static string Repeat(this char chatToRepeat, int repeat)
        {

            return new string(chatToRepeat, repeat);
        }
        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }
    }
}
