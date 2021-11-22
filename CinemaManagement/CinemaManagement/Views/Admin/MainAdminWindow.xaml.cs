using System.Windows;
using System.Windows.Input;

namespace CinemaManagement
{
    /// <summary>
    /// Interaction logic for MainAdminWindow.xaml
    /// </summary>
    public partial class MainAdminWindow : Window
    {
        public MainAdminWindow()
        {
            InitializeComponent();
        }

        private void mainadminwindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMove();
        }

        private void SlideButton_Checked(object sender, RoutedEventArgs e)
        {
            topnotifi.Visibility = Visibility.Collapsed;
        }

        private void SlideButton_Unchecked(object sender, RoutedEventArgs e)
        {
            topnotifi.Visibility = Visibility.Visible;
        }
    }
}
