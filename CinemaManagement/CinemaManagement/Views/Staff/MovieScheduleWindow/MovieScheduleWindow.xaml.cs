using CinemaManagement.DTOs;
using CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaManagement.Views.Staff.MovieScheduleWindow
{

    public partial class MovieScheduleWindow : Window
    {
        Border ShowTimeSelected = null;

        public MovieScheduleWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
