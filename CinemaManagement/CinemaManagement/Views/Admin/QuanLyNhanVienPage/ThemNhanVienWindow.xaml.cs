using System.Windows;

namespace CinemaManagement.Views.Admin.QuanLyNhanVienPage
{
    /// <summary>
    /// Interaction logic for ThemNhanVienWindow.xaml
    /// </summary>
    public partial class ThemNhanVienWindow : Window
    {
        public ThemNhanVienWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
