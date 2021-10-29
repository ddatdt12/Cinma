using CinemaManagement.ViewModel.AdminVM.MovieManagementVM;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.QuanLyPhimPage
{
    /// <summary>
    /// Interaction logic for AddMovieWindow.xaml
    /// </summary>
    public partial class AddMovieWindow : Window
    {
        public AddMovieWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void addmoviewindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Escape) return;

            e.Handled = true;
            this.Close();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_movieName.Text))
                _movieName.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieGenre.Text))
                _movieGenre.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieGenre.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieDirector.Text))
                _movieDirector.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieDirector.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieCountry.Text))
                _movieCountry.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieCountry.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieDuration.Text))
                _movieDuration.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieDuration.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieYear.Text))
                _movieYear.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieYear.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (string.IsNullOrWhiteSpace(_movieDes.Text))
                _movieDes.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                _movieDes.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFA5B9D6");

            if (imgframe.Source == null)
                MessageBox.Show("Ảnh không được trống");

        }
    }
}
