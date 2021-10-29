using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.QuanLyPhimPage
{
    /// <summary>
    /// Interaction logic for EditMovie.xaml
    /// </summary>
    public partial class EditMovie : Window
    {
        public EditMovie()
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

        private void Duration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(_Name.Text))
                _Name.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Name.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_Genre.Text))
                _Genre.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Genre.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_Author.Text))
                _Author.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Author.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_Country.Text))
                _Country.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Country.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_Duration.Text))
                _Duration.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Duration.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(Year.Text))
                Year.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                Year.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_Descripstion.Text))
                _Descripstion.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _Descripstion.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (imgframe.Source == null)
                MessageBox.Show("Ảnh không được trống");

        }
    }
}
