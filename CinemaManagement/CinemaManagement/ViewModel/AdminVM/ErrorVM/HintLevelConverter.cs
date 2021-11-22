using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.ErrorVM
{
    public class HintLevelConverter : IValueConverter
    {// This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == Utils.LEVEL.CRITICAL)
                return Visibility.Visible;
            else if (text == Utils.LEVEL.NORMAL)
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HintCountConverter : IValueConverter
    {// This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            
            string text = value as string;

            if (text == "0")
                return Visibility.Collapsed;
            else if (text != "0")
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
