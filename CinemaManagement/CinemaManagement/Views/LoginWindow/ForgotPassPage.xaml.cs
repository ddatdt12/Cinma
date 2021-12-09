using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CinemaManagement.Views.LoginWindow
{
    public partial class ForgotPassPage : Page
    {
        public ForgotPassPage()
        {
            InitializeComponent();
        }
        private bool ismove = false;
        private void sendmailbtn_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassViewModel.ForgotPasswordEmail = null;
            ForgotPassViewModel.RequestingStaffId = null;
            string requestUsername = requestUsernameField.Text;
            if (string.IsNullOrEmpty(requestUsernameField.Text) && Username.IsEnabled == true)
            {
                errorlbl.Content = "Không được để trống!";
                return;
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

            if (!ismove)
                Move1();
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

            Move2();
            _Instruct.Text = "Chúng tôi sẽ gửi mã đặt lại mật khẩu thông qua Email liên kết với tài khoản trên";
        }


        public void Move1()
        {
            var sb = new Storyboard();
            var ta = new ThicknessAnimation();
            ta.BeginTime = new TimeSpan(0);
            ta.SetValue(Storyboard.TargetNameProperty, "topobj");
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

            ta.From = new Thickness(0, 0, 0, 70);
            ta.To = new Thickness(0, 50, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            sb.Children.Add(ta);
            sb.Begin(this);

            var sb2 = new Storyboard();
            var ta2 = new ThicknessAnimation();
            ta2.BeginTime = new TimeSpan(0);
            ta2.SetValue(Storyboard.TargetNameProperty, "_Instruct");
            Storyboard.SetTargetProperty(ta2, new PropertyPath(MarginProperty));

            ta2.From = new Thickness(0, 0, 0, 50);
            ta2.To = new Thickness(0, 0, 0, 150);
            ta2.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            sb2.Children.Add(ta2);
            sb2.Begin(this);

            ismove = true;
        }
        public void Move2()
        {
            var sb = new Storyboard();
            var ta = new ThicknessAnimation();
            ta.BeginTime = new TimeSpan(0);
            ta.SetValue(Storyboard.TargetNameProperty, "topobj");
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

            ta.From = new Thickness(0, 50, 0, 0);
            ta.To = new Thickness(0, 0, 0, 70);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            sb.Children.Add(ta);
            sb.Begin(this);

            var sb2 = new Storyboard();
            var ta2 = new ThicknessAnimation();
            ta2.BeginTime = new TimeSpan(0);
            ta2.SetValue(Storyboard.TargetNameProperty, "_Instruct");
            Storyboard.SetTargetProperty(ta2, new PropertyPath(MarginProperty));

            ta2.From = new Thickness(0, 0, 0, 150);
            ta2.To = new Thickness(0, 0, 0, 50);
            ta2.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            sb2.Children.Add(ta2);
            sb2.Begin(this);

            ismove = false;
        }
    }
}
