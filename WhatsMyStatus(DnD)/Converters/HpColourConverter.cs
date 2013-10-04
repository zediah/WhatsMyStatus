using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WhatsMyStatus_DnD_.Coverters
{
    public class HpColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int val = (int)value;
                if (val < 0)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                return new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Purple);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
