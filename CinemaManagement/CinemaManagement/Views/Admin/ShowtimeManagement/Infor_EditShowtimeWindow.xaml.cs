using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.ShowtimeManagement
{

    public partial class Infor_EditShowtimeWindow : Window
    {
        Border SelectedShowtime = null;
        
        public Infor_EditShowtimeWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        //    this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectedShowtime != null)
                SelectedShowtime.Background = new SolidColorBrush(Colors.Wheat);

            SelectedShowtime = (Border)sender;

            SelectedShowtime.Background = new SolidColorBrush(Colors.LightBlue);
        }


        bool IsEdit = false;
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsEdit = !IsEdit;

            if (IsEdit)
            {
                _showtimePrice.IsEnabled = true;
                _lblEdit.Content = "Lưu";
            }
            else
            {
                _showtimePrice.IsEnabled = false ;
                _lblEdit.Content = "Thay đổi";
            }

        }
        private void _showtimePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
