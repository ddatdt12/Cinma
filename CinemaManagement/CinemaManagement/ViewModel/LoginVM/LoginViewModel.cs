﻿using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.ViewModel.AdminVM.VoucherManagementVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketBillVM;
using CinemaManagement.Views.LoginWindow;
using CinemaManagement.Views.Staff;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public Button LoginBtn { get; set; }
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
        public ICommand SaveLoginBtnCM { get; set; }


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
                (_, _, _) = StaffService.Ins.GetStaffEmail("");
            }
            catch (InvalidOperationException)
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
            LoginCM = new RelayCommand<Label>((p) => { return true; }, async (p) =>
             {
                 //Bên quản lý Customer
                 //Theo năm
                 var byYear = await CustomerService.Ins.GetAllCustomerByTime(2021);

                 //Theo tháng
                 var byMonth = await CustomerService.Ins.GetAllCustomerByTime(2021, 11);

                 //Dành cho thống kê
                 //Theo tháng
                 (int NewCustomerQuanity, int TotalCustomerQuantity, int WalkinGuestQuantity) = await StatisticsService.Ins.GetDetailedCustomerStatistics(2021, 11);

                 //Theo năm
                 (int NewCustomerQuanityInYear, int TotalCustomerQuantityInYear, int WalkinGuestQuantityInYear) = await StatisticsService.Ins.GetDetailedCustomerStatistics(2021);


                 //
                 //CustomerDTO updated = new CustomerDTO
                 //{
                 //    Id = "KH0001",
                 //    Name = "Đỗ Tiến Đạt",
                 //    PhoneNumber = "09876582123",
                 //    Email = "ddatdt12@gmail.com",
                 //};
                 //(bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.UpdateCustomerInfo(updated);
                 //(bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.DeleteCustomer(CustomerId);
                 // 

                 string username = Username;    
                 string password = Password;

                 IsLoading = true;

                 await CheckValidateAccount(username, password, p);

                 IsLoading = false;
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
            SaveLoginBtnCM = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                LoginBtn = p;
            });
        }

        public async Task CheckValidateAccount(string usn, string pwr, Label lbl)
        {

            if (string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(pwr))
            {
                lbl.Content = "Vui lòng nhập đủ thông tin";
                return;
            }


            LoginBtn.Content = "";
            LoginBtn.IsHitTestVisible = false;
            LoginPage.pgb.Visibility = Visibility.Visible;

            (bool loginSuccess, string message, StaffDTO staff) = await Task<(bool loginSuccess, string message, StaffDTO staff)>.Run(() => StaffService.Ins.Login(usn, pwr));

            LoginBtn.Content = "Đăng nhập";
            LoginBtn.IsHitTestVisible = true;
            LoginPage.pgb.Visibility = Visibility.Collapsed;
            if (loginSuccess)
            {

                Password = "";
                TicketBillViewModel.Staff = staff;
                LoginWindow.Hide();
                if (staff.Role == "Quản lý")
                {
                    MainAdminWindow w1 = new MainAdminWindow();
                    MainAdminViewModel.currentStaff = staff;
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
