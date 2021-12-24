using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.ErrorManagement
{
    public partial class WaitingError : Window
    {
        public WaitingError()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
            costval.Text = "0";
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
            btn.Background = new SolidColorBrush(Colors.OrangeRed);
        }
        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;

            if (cbb.SelectedValue.ToString() == "Đã hủy")
            {
                _Finishday.Visibility = Visibility.Collapsed;
                _startday.Visibility = Visibility.Collapsed;
                _cost.Visibility = Visibility.Collapsed;
            }
            else if (cbb.SelectedValue.ToString() == "Đang giải quyết")
            {
                _startday.IsEnabled = true;
                _cost.IsEnabled = false;
                _Finishday.IsEnabled = false;
                costval.Text = "0";
                _Finishday.Visibility = Visibility.Collapsed;
                _startday.Visibility = Visibility.Visible;
                _cost.Visibility = Visibility.Collapsed;
            }
            else if (cbb.SelectedValue.ToString() == "Đã giải quyết")
            {
                _startday.IsEnabled = false;
                start.SelectedDate = System.DateTime.Today;
                _Finishday.IsEnabled = true;
                _cost.IsEnabled = true;
                _Finishday.Visibility = Visibility.Visible;
                _startday.Visibility = Visibility.Visible;
                _cost.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                tb.Text = "0";
        }
    }
}
