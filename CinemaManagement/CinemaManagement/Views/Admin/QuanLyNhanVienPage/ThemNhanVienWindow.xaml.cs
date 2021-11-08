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
            this.Owner = App.Current.MainWindow;
        }

        private void ThemNV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMove();
        }
    }
}
