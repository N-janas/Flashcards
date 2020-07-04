using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace FlashCards.ViewModel
{
    class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value == true)
                return System.Windows.Visibility.Visible;
            return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((System.Windows.Visibility)value == System.Windows.Visibility.Hidden)
                return false;
            return true;
        }
    }
}
