using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.QuanLyNhanVienPage
{
    public partial class ThemNhanVienWindow : Window
    {
        public ThemNhanVienWindow()
        {
            InitializeComponent();
            //this.Owner = App.Current.MainWindow;
        }

        private void ThemNV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMove();
        }

        private void _Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
