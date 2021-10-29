using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.QuanLySuatChieuPage
{
    /// <summary>
    /// Interaction logic for AddSuatChieuWindow.xaml
    /// </summary>
    public partial class AddSuatChieuWindow : Window
    {
        public AddSuatChieuWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_movieName.Text))
                _movieName.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieDate.Text))
                _movieDate.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieDate.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");
            if (string.IsNullOrWhiteSpace(_movieRoom.Text))
                _movieRoom.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieRoom.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");
            if (_movieTime.Text == "0:00")
                _movieTime.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieTime.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");
        }
    }
}
