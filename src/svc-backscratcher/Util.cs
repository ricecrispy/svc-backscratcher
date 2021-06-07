using svc_backscratcher.Models;
using System;

namespace svc_backscratcher
{
    public static class Util
    {
        public static double GetProductPrice(string price)
        {
            TryGetProductPrice(price, out double parsedPrice);
            return parsedPrice;
        }

        public static bool TryGetProductPrice(string priceString, out double parsedPrice)
        {
            return double.TryParse(priceString.Replace("$", ""), out parsedPrice);
        }

        public static bool TryConvertSize(string size, out BackScratcherSize parsedSize)
        {
            return Enum.TryParse(size.ToUpper().Trim(), out parsedSize);
        }
    }
}
