using CinemaManagement.DTOs;
using CinemaManagement.Models;
using CinemaManagement.Models.Services;
using CinemaManagement.ViewModel.AdminVM.VoucherManagementVM;
using CinemaManagement.Views;
using CinemaManagement.Views.LoginWindow;
using CinemaManagement.Views.Staff;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CinemaManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public static Frame MainFrame { get; set; }
        public Window LoginWindow { get; set; }
        public ICommand ShadowMaskCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand ForgotPassCM { get; set; }
        public ICommand LoginCM { get; set; }
        public ICommand PasswordChangedCM { get; set; }
        public ICommand LoadLoginPageCM { get; set; }
        public ICommand SaveLoginWindowNameCM { get; set; }


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

        private bool isloadding;
        public bool IsLoading
        {
            get { return isloadding; }
            set { isloadding = value; OnPropertyChanged(); }
        }


        public LoginViewModel()
        {
            try
            {
            }
            catch (InvalidOperationException e)
            {
            }
            catch (Exception e)
            {
                MessageBox.Show($"Mất kết nối cơ sở dữ liệu! Vui lòng kiểm tra lại", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MouseLeftButtonDownWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (p != null)
                {
                    p.DragMove();
                }
            });
            ForgotPassCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                MainFrame.Content = new ForgotPassPage();
            });
            LoginCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                string username = Username;
                string password = Password;

                CheckValidateAccount(username, password, p);
            });
            PasswordChangedCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            });
            LoadLoginPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
              {
                  MainFrame = p;
                  p.Content = new LoginPage();
              });
            SaveLoginWindowNameCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                LoginWindow = p;
            });
        }

        public void CheckValidateAccount(string usn, string pwr, Label lbl)
        {

            if (string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(pwr))
            {
                lbl.Content = "Vui lòng nhập đủ thông tin";
                return;
            }

            (bool loginSuccess, string message, StaffDTO staff) = StaffService.Ins.Login(usn, pwr);
            if (loginSuccess)
            {

                Password = "";
                VoucherViewModel.StaffID = staff.Id;
                LoginWindow.Hide();
                if (staff.Role == "Quản lý")
                {
                    MainAdminWindow w1 = new MainAdminWindow();
                    w1.CurrentUserName.Content = staff.Name;
                    w1.Show();
                    LoginWindow.Close();
                    return;
                }
                else
                {
                    MainStaffWindow w1 = new MainStaffWindow();
                    w1._StaffName.Text = staff.Name;
                    w1.Show();
                    LoginWindow.Close();
                    return;
                }

            }
            else
            {
                lbl.Content = message;
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
