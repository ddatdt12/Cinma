using CinemaManagement.Utils;
using CinemaManagement.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.Views.LoginWindow
{
    public partial class ForgotPassPage : Page
    {
        public ForgotPassPage()
        {
            InitializeComponent();
        }

        private void sendmailbtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailfield.Text) && email.IsEnabled == true)
            {
                errorlbl.Content = "Không được để trống!";
            }
            else if (true) //check if account is exists 
            {
                //ForgotPassViewModel.IsExistsAccount = ....
                MessageBoxCustom mb = new MessageBoxCustom("", "Tài khoản không tồn tại!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();

            }
            else
            {
                errorlbl.Content = "";
                havecode.Visibility = Visibility.Visible;
                sendmailbtn.Visibility = Visibility.Collapsed;
                acceptbutn.Visibility = Visibility.Visible;
                secretcode.IsEnabled = true;
                email.IsEnabled = false;
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock t = sender as TextBlock;

            this.Cursor = Cursors.Hand;
            t.Foreground = new SolidColorBrush(Colors.Blue);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock t = sender as TextBlock;

            this.Cursor = Cursors.Arrow;
            t.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void acceptbutn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codefield.Password))
            {
                MessageBox.Show("Không hợp lệ!");
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            errorlbl.Content = "";
            havecode.Visibility = Visibility.Collapsed;
            sendmailbtn.Visibility = Visibility.Visible;
            acceptbutn.Visibility = Visibility.Collapsed;
            codefield.Password = "";
            secretcode.IsEnabled = false;
            email.IsEnabled = true;
        }
    }
}
