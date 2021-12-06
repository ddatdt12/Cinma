using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.LoginWindow;
using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.IdentityModel.Protocols;

namespace CinemaManagement.ViewModel
{

    public class ForgotPassViewModel : BaseViewModel
    {
        public Button SendmailBtn { get; set; }
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
        public ICommand SaveSendmailBtnCM { get; set; }

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

                 if (string.IsNullOrEmpty(usremail)) return;

                 if (!RegexUtilities.IsValidEmail(usremail)) return;

                 //(bool isSuccess, string message)  = await sendHtmlEmail();
                 //MessageBox.Show(message);

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
            SaveSendmailBtnCM = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                SendmailBtn = p;
            });
        }
        protected async Task<(bool, string)> sendHtmlEmail()
        {
            string to1 = "binzml1714@gmail.com";
            string to2 = "ddatdt12@gmail.com";

            List<string> listCode1 = new List<string> { "CODE1", "CODE2", "CODE3", "CODE4" };
            List<string> listCode2 = new List<string> { "CODE5", "CODE6", "CODE7", "CODE8" };


            Task t1 = sendEmailForACustomer(to1, listCode1);
            Task t2 = sendEmailForACustomer(to2, listCode2);

            try
            {
                await Task.WhenAll(t1, t2);
                return (false, "Gửi thành công");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

        }
        protected Task sendEmailForACustomer(string customerEmail, List<string> listCode)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string APP_EMAIL = appSettings["APP_EMAIL"];
            string APP_PASSWORD = appSettings["APP_PASSWORD"];

            //SMTP CONFIG
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            //create Alrternative HTML view

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetCustomerGratitudeTemplate(listCode), null, "text/html");
            //Add Image
            LinkedResource theEmailImage = new LinkedResource(Helper.GetImagePath("poster.png"));
            theEmailImage.ContentId = "myImageID";


            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImage);
            //Add view to the Email Message
            mail.AlternateViews.Add(htmlView);

            mail.From = new MailAddress(FROM_EMAIL, "Squadin Cinema");
            mail.To.Add(customerEmail);
            mail.Subject = "Tri ân khách hàng thân thiết";

            return smtp.SendMailAsync(mail);
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
            String HTML = File.ReadAllText(resetPasswordHTML).Replace("RESET_PASSWORD_CODE", "123123");
            return HTML;
        }

        private string GetCustomerGratitudeTemplate(List<string> listCode)
        {
            string templateHTML = Helper.GetEmailTemplatePath(GRATITUDE_TEMPLATE_FILE);
            string listVoucherHTML = "";

            for (int i = 0; i < listCode.Count; i++)
            {
                listVoucherHTML += VOUCHER_ITEM_HTML.Replace("{INDEX}", $"{i + 1}").Replace("{CODE_HERE}", listCode[i]);
            }


            String HTML = File.ReadAllText(templateHTML).Replace("{LIST_CODE_HERE}", listVoucherHTML);
            return HTML;
        }

        string FROM_EMAIL = "squandincinema@gmail.com";
        string PASS_EMAIL = "khongcopass@2k2";

        const string RESET_PASS_TEMPLATE_FILE = "reset_password_html.txt";
        const string GRATITUDE_TEMPLATE_FILE = "top5_customer_gratitude_html.txt";

        const string VOUCHER_ITEM_HTML = "<li>Voucher {INDEX}: {CODE_HERE}</li>";
    }
}
