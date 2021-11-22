using System;
using System.Globalization;
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

            switch (text)
            {
                //Implement your logic here
                case "Chưa sửa":
                    return new SolidColorBrush(Colors.Red);
                case "Đã sửa":
                    return new SolidColorBrush(Colors.Green);
                case "Đang sửa chữa":
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
