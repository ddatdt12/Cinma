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
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedShowtime != null)
                SelectedShowtime.Background = new SolidColorBrush(Colors.Wheat);

            SelectedShowtime = (Border)sender as Border;

            SelectedShowtime.Background = new SolidColorBrush(Colors.LightBlue);
        }
    }
}
