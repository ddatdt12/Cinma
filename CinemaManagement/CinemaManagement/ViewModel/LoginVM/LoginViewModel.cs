using CinemaManagement.Models;
using CinemaManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand ForgotPassCM { get; set; }
        public ICommand LoginCM { get; set; }
        public ICommand PasswordChangedCM { get; set; }


        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }


        public LoginViewModel()
        {

            CloseWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {

                if (p != null)
                {
                    p.Close();
                }

            });
            MinimizeWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {

                if (p != null)
                {
                    p.WindowState = WindowState.Minimized;
                }
            });
            MouseLeftButtonDownWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (p != null)
                {
                    p.DragMove();
                }
            });
            ForgotPassCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                ForgotPassWindow w1 = new ForgotPassWindow();
                w1.ShowDialog();
            });

            LoginCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                string username = Username;
                string password = Password;

                if (CheckValidateAccount(username, password))
                {
                    p.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    p.Foreground = new SolidColorBrush(Colors.Red);
                }
            });

            PasswordChangedCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            });
        }

        public bool CheckValidateAccount(string usn, string pwr)
        {
            if (usn == "1" && pwr == "1")
                return true;
            return false;
        }

    }
}
