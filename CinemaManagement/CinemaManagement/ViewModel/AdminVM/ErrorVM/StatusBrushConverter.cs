using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.ErrorVM
{
    public class StatusBrushConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == Utils.STATUS.WAITING)
                return new SolidColorBrush(Colors.Red);
            else if (text == Utils.STATUS.DONE)
                return new SolidColorBrush(Colors.ForestGreen);
            else if (text == Utils.STATUS.IN_PROGRESS)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#DDC1C107");
            else
                return new SolidColorBrush(Colors.Gray);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusEditConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == Utils.STATUS.WAITING)
                return Visibility.Visible;
            else if (text == Utils.STATUS.DONE)
                return Visibility.Visible;
            else if (text == Utils.STATUS.IN_PROGRESS)
                return Visibility.Visible;
            else
                return Visibility.Hidden;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
