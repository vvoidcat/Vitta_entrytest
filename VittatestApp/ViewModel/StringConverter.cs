using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VittatestApp.ViewModel
{
    static class StringConverter
    {
        public static decimal StringToDecimal(string str)
        {
            return (Decimal.TryParse(str, out decimal res)) ? res : 0;
        }

        public static long StringToLong(string str)
        {
            return (Int64.TryParse(str, out long res)) ? res : 0;
        }
    }
}
