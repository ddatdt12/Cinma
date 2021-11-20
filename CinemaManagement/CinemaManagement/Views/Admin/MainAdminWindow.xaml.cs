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
    }
}
