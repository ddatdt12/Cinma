using System.Windows;
using System.Windows.Input;

namespace CinemaManagement
{
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

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MainFrame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SlideButton.IsChecked = false;
        }
        private void SlideButton_Checked(object sender, RoutedEventArgs e)
        {
            topnotifi.Visibility = Visibility.Collapsed;
        }

        private void SlideButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (count.Text != "0")
                topnotifi.Visibility = Visibility.Visible;
        }
    }
}
