using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.QuanLyPhimPage
{
    /// <summary>
    /// Interaction logic for InforMovieWindow.xaml
    /// </summary>
    public partial class InforMovieWindow : Window
    {
        public InforMovieWindow()
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
    }
}
