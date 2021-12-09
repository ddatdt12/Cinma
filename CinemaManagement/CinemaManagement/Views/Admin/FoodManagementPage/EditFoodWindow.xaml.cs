using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagement.Views.Admin.FoodManagementPage
{
    public partial class EditFoodWindow : Window
    {
        public EditFoodWindow()
        {
            InitializeComponent();
            //this.Owner = App.Current.MainWindow;
        }

        private void EditFoodWd_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void _price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if (t.Text.Length <= 0)
                t.Text = "0";
        }

    }
}
