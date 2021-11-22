using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CinemaManagement.ViewModel.AdminVM.ErrorViewModel
{
    public class ErrorViewModel : BaseViewModel
    {
        private List<Error> listError;
        public List<Error> ListError
        {
            get { return listError; }
            set { listError = value; OnPropertyChanged(); }
        }

        public ErrorViewModel()
        {
            ListError = new List<Error>();

            Error t = new Error
            {
                Detail = "sdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdsssssssssssssssssssdssssssssssssssssss",
                Name = "Sua micro di~~~",
                Status = "Đang sửa chữa",
                Fix = false
            };

            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);
            ListError.Add(t);

        }


    }

    public class Error
    {
        //public string name;
        //public string detail;
        //public ImageSource img;
        //public string status;
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string detail;
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private bool fix;
        public bool Fix
        {
            get { return fix; }
            set { fix = value; }
        }

    }

    public class BrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                {
                    return System.Windows.Media.Colors.Black;
                }
            }
            return System.Windows.Media.Colors.LightGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
