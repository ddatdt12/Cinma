using CinemaManagement.ViewModel.AdminVM.VoucherManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaManagement.Views.Admin.VoucherManagement.Infor_EditWindow
{
    /// <summary>
    /// Interaction logic for Edit_InforPage.xaml
    /// </summary>
    public partial class Edit_InforPage : Page
    {
        public Edit_InforPage()
        {
            InitializeComponent();
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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                tb.Text = "0";
        }

        private void yes_Checked(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status2 = true;
        }

        private void no_Checked(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status2 = false;
        }
    }
}
