using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.LoginWindow;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
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
            SendMailCM = new RelayCommand<Button>((p) => { return true; }, async (p) =>
             {
                 IsLoading = true;
                 p.IsEnabled = false;
                 p.Content = "";
                 (bool IsSucess ,string message)= await sendHtmlEmail();
                 IsLoading = false;
                 p.IsEnabled = true;
                 p.Content = "Gửi mã";
                 MessageBox.Show(message);

                 //string to, from, pass, messageBody;
                 //MailMessage message = new MailMessage();
                 //string to1 = "binzml1714@gmail.com";
                 //string to2 = "ddatdt12@gmail.com";
                 //from = "20521163@gm.uit.edu.vn";
                 //pass = "NoLove1205@";
                 //messageBody = "";
                 //message.To.Add(to1);
                 //message.To.Add(to2);
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
        protected async Task SendEmail()
        {
            string Themessage = @"<html>
                              <body>
                                <table width=""100%"">
                                <tr>
                                    <td style=""font-style:arial; color:maroon; font-weight:bold"">
                                   Hi! <br>
                                    <img src=cid:myImageID>
                                    </td>
                                </tr>
                                </table>
                                </body>
                                </html>";

            string content = @"";
            //await sendHtmlEmail(Themessage);
        }
        protected async Task<(bool, string)> sendHtmlEmail()
        {
            //create an instance of new mail message
            MailMessage mail = new MailMessage();

            //set the HTML format to true
            mail.IsBodyHtml = true;

            //create Alrternative HTML view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetResetPasswordTemplate(), null, "text/html");

            //Add Image
            LinkedResource theEmailImage = new LinkedResource(Helper.GetImagePath("poster.png"));
            theEmailImage.ContentId = "myImageID";

            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImage);

            //Add view to the Email Message
            mail.AlternateViews.Add(htmlView);

            //set the SMTP info
            string to1 = "binzml1714@gmail.com";
            string to2 = "ddatdt12@gmail.com";

            string FROM_EMAIL = "squandincinema@gmail.com";
            string PASS_EMAIL = "khongcopass@2k2";

            //set the "from email" address and specify a friendly 'from' name
            mail.From = new MailAddress(FROM_EMAIL, "Squadin Cinema");

            //set the "to" email address
            mail.To.Add(to1);
            mail.To.Add(to2);

            //set the Email subject
            mail.Subject = "Gửi mail tri ân";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FROM_EMAIL, PASS_EMAIL);

            //send the email
            try
            {
                //await smtp.SendMailAsync(mail);
                await Task.Delay(5000);
                return (true, "Gửi email thành công!");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
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


        private string GetResetPasswordTemplate()
        {
            string resetPasswordHTML = Helper.GetEmailTemplatePath(RESET_PASS_TEMPLATE_FILE);
            String sHTML = File.ReadAllText(resetPasswordHTML).Replace("RESET_PASSWORD_CODE", "123123");
            return sHTML;
        }
        const string RESET_PASS_TEMPLATE_FILE = "reset_password_html.txt";
    }
}
