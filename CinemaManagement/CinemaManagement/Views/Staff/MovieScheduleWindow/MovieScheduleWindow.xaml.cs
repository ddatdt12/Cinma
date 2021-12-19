using CinemaManagement.ViewModel;
using CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.Staff.MovieScheduleWindow
{

    public partial class MovieScheduleWindow : Window
    {
        Border ShowTimeSelected = null;

        public MovieScheduleWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShowTimeSelected != null)
                ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff97c5");

            ShowTimeSelected = (Border)sender;

            ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF6378B");
            if (_Room.Visibility == Visibility.Collapsed)
                _Room.Visibility = Visibility.Visible;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            _Room.Visibility = Visibility.Collapsed;
        }

        private void Movie_Schedule_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
