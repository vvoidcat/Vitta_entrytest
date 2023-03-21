using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VittatestApp.ViewModel
{
    class InsertionParamsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<string, string, string> tuple = new Tuple<string, string, string>((string)values[0], (string)values[1], (string)values[2]);
            return tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is null && value is not Tuple<string, string, string>)
            {
                throw new ArgumentNullException("value is null / value is not Tuple<string, string, string>");
            }

            Tuple<string, string, string> tuple = (Tuple<string, string, string>)value;
            object[] convResult = new object[3];
            convResult[0] = tuple.Item1;
            convResult[1] = tuple.Item2;
            convResult[2] = tuple.Item3;
            return convResult;
        }
    }
}
