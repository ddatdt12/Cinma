using CinemaManagement.Views.LoginWindow;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{

    public class ForgotPassViewModel : BaseViewModel
    {

        private string _usremail;
        public string usremail
        {
            get { return _usremail; }
            set { _usremail = value; OnPropertyChanged(); }
        }

        private string _code;
        public string code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged(); }
        }

        private string newPass;
        public string NewPass
        {
            get { return newPass; }
            set { newPass = value; OnPropertyChanged(); }
        }



        public ICommand ConfirmCM { get; set; }
        public ICommand CancelCM { get; set; }
        public ICommand SendMailCM { get; set; }
        public ICommand CodeChangedCM { get; set; }
        public ICommand PreviousPageCM { get; set; }
        public ICommand SaveNewPassCM { get; set; }
        public ICommand NewPassChanged { get; set; }

        public ForgotPassViewModel()
        {
            CancelCM = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) =>
            {
                LoginViewModel.MainFrame.Content = new LoginPage();
            });
            ConfirmCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                if (!string.IsNullOrEmpty(p.Password))
                {
                    if (true)
                    {
                        LoginViewModel.MainFrame.Content = new ChangePassPage();
                    }
                    else
                    {

                    }
                }

            });
            PreviousPageCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                LoginViewModel.MainFrame.Content = new ForgotPassPage();
            });
            CodeChangedCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                code = p.Password;
            });
            SendMailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                //string to, from, pass, messageBody;
                //MailMessage message = new MailMessage();
                //to = "20520594@gm.uit.edu.vn";
                //from = "20520594@gm.uit.edu.vn";
                //pass = "passhere";
                //messageBody = "this is body";
                //message.To.Add(to);
                //message.From = new MailAddress(from);
                //message.Body = "From : " + "<br>Message: " + messageBody;
                //message.Subject = "this is subject";
                //message.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //smtp.EnableSsl = true;
                //smtp.Port = 587;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = new NetworkCredential(from, pass);

                //try
                //{
                //    smtp.Send(message);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

            });
            SaveNewPassCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(NewPass))
                {
                    p.Content = "Không hợp lệ!";
                    p.Visibility = Visibility.Visible;
                }
                else
                {
                    p.Content = "Thay đổi mật khẩu thành công!";
                    p.Visibility = Visibility.Visible;
                }
            });
            NewPassChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                NewPass = p.Password;
            });
        }

        FrameworkElement GetParentWindow(FrameworkElement p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
