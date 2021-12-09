using System.Text.RegularExpressions;
using System.Windows;

namespace CinemaManagement.Views.Admin.QuanLyNhanVienPage
{
    public partial class SuaNhanVienWindow : Window
    {
        public SuaNhanVienWindow()
        {
            InitializeComponent();
            //this.Owner = App.Current.MainWindow;
        }

        private void _Phone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
