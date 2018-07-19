using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using wpfTask1;

namespace WpfApp1
{
    class MailConverter : IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //string s1 = values[0] as string;
            string s1 = "";
            string s2 = values[1] as string;
            return string.Format("{0}{1}", s1, s2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
