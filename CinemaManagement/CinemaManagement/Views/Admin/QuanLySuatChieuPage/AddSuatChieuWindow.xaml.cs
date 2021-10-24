using System.Windows;
using System.Windows.Input;

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
    }
}
