using System;
using System.Globalization;

using Windows.UI;

namespace AccountingManager.Helpers
{
    class HexToColor
    {
        public static Color Convet(string hex)
        {
            Byte a = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            Byte r = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            Byte g = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
            Byte b = byte.Parse(hex.Substring(7, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
