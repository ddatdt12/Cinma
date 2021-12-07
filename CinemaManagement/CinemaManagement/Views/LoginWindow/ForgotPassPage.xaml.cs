using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(requestUsernameField.Text) && Username.IsEnabled == true)
            {
                errorlbl.Content = "Không được để trống!";
            }
            else
            {
                (string error, string staffEmail, string staffId) = StaffService.Ins.GetStaffEmail(requestUsername);

                if (error != null)
                {
                    errorlbl.Content = error;
                    return;
                }
                else
                {
                    ForgotPassViewModel.ForgotPasswordEmail = staffEmail;
                    ForgotPassViewModel.RequestingStaffId = staffId;
                }
                //check if account exists 
                if (!RegexUtilities.IsValidEmail(staffEmail))
                {
                    errorlbl.Content = "Tài khoản không tồn tại Email\nLiên hệ quản trị viên";
                    return;
                }
                else
                {
                    errorlbl.Content = "";
                    havecode.Visibility = Visibility.Visible;
                    sendmailbtn.Visibility = Visibility.Collapsed;
                    acceptbutn.Visibility = Visibility.Visible;
                    secretcode.Visibility = Visibility.Visible;
                    Username.Visibility = Visibility.Collapsed;
                }

            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void acceptbutn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codefield.Password))
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Không hợp lệ!", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            errorlbl.Content = "";
            havecode.Visibility = Visibility.Collapsed;
            sendmailbtn.Visibility = Visibility.Visible;
            acceptbutn.Visibility = Visibility.Collapsed;
            codefield.Password = "";
            secretcode.Visibility = Visibility.Collapsed;
            Username.Visibility = Visibility.Visible;
        }
    }
}
