using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.QuanLyNhanVienPage
{
    public partial class ThemNhanVienWindow : Window
    {
        public ThemNhanVienWindow()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        private void ThemNV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMove();
        }
    }
}
