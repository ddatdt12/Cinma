using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CinemaManagement.ViewModel.StaffViewModel.TroubleWindowVM
{
    public class EditConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == Utils.STATUS.WAITING)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
