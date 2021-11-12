using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Staff;
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

                Window temp = GetParentWindow(p) as Window;

                CheckValidateAccount(username, password, temp, p);


            });

            PasswordChangedCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            });
        }

        public void CheckValidateAccount(string usn, string pwr, Window p, Label lbl)
        {

            if (string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(pwr))
            {
                lbl.Content = "Vui lòng nhập đủ thông tin";
                return;
            }


            (bool loginSuccess, string message, StaffDTO staff) = StaffService.Ins.Login(usn, pwr);

            if (loginSuccess)
            {
                if (staff.Role == "Quản lý")
                {
                    MainAdminWindow w1 = new MainAdminWindow();
                    w1.CurrentUserName.Content = staff.Name;
                    p.Hide();
                    w1.ShowDialog();
                    p.Close();
                    return;
                }
                else
                {
                    MainStaffWindow w1 = new MainStaffWindow();
                    p.Hide();
                    w1.ShowDialog();
                    p.Close();
                    return;
                }

            }
            else
            {
                lbl.Content = "Sai tài khoản hoặc mật khẩu";
                return;
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

    }
}
