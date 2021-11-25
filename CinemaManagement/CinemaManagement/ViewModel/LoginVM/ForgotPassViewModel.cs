using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{

    public class ForgotPassViewModel
    {
        public ICommand ConfirmCM { get; set; }
        public ICommand CancelCM { get; set; }



        public ForgotPassViewModel()
        {
            CancelCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });

            ConfirmCM = new RelayCommand<object>((p) => { return true; }, (p) =>
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
