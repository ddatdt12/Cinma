using CinemaManagement.Models.Services;
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

            string requestUsername = requestUsernameField.Text;
            if (string.IsNullOrEmpty(requestUsernameField.Text) && email.IsEnabled == true)
            {
                errorlbl.Content = "Không được để trống!";
            }
            else
            { 
                (string error, string staffEmail, string staffId) = StaffService.Ins.GetStaffEmail(requestUsername);

                ForgotPassViewModel.ForgotPasswordEmail = staffEmail;
                ForgotPassViewModel.RequestingStaffId = staffId;

                if (error != null)
                {
                    //Có thể chuyển qua thông báo lỗi chỗ label cũng được
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", error, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                //check if account exists 
                if (!RegexUtilities.IsValidEmail(staffEmail))
                {
                    //Có thể chuyển qua thông báo lỗi chỗ label cũng được
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Email đăng kí của tài khoản này không hợp lệ. Vui lòng liên hệ quản lý để được hỗ trợ", MessageType.Warning, MessageButtons.OK);
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
