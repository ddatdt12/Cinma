using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.ErrorManagement;
using CinemaManagement.Views.Admin.FoodManagementPage;
using CinemaManagement.Views.Admin.Import_ExportManagement;
using CinemaManagement.Views.Admin.MovieManagement;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using CinemaManagement.Views.Admin.ShowtimeManagementVM;
using CinemaManagement.Views.Admin.StatisticalManagement;
using CinemaManagement.Views.LoginWindow;
using System;
using System.Collections.Generic;
using CinemaManagement.Views.Admin.VoucherManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CinemaManagement.Views;
using System.Threading.Tasks;
using CinemaManagement.Views.Admin.CustomerManagement;
using CinemaManagement.Views.Staff;

namespace CinemaManagement.ViewModel
{

    public partial class MainAdminViewModel : BaseViewModel
    {
        public static StaffDTO currentStaff;
        public ICommand SignoutCM { get; set; }
        public ICommand LoadQLPPageCM { get; set; }
        public ICommand LoadQLNVPageCM { get; set; }
        public ICommand LoadSuatChieuPageCM { get; set; }
        public ICommand LoadLSPage { get; set; }
        public ICommand LoadTKPageCM { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand LoadErrorPage { get; set; }
        public ICommand LoadVCPageCM { get; set; }
        public ICommand LoadQLKHPageCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand ChangeRoleCM { get; set; }

        private string _SelectedFuncName;
        public string SelectedFuncName
        {
            get { return _SelectedFuncName; }
            set { _SelectedFuncName = value; OnPropertyChanged(); }
        }

        private string _ErrorCount;
        public string ErrorCount
        {
            get { return _ErrorCount; }
            set { _ErrorCount = value; OnPropertyChanged(); }
        }


        public MainAdminViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 SelectedFuncName = "Quản lý suất chiếu";
                 await CountErrorFunc();
             });
            SignoutCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
               {
                   FrameworkElement window = GetParentWindow(p);
                   var w = window as Window;
                   if (w != null)
                   {
                       w.Hide();
                       LoginWindow w1 = new LoginWindow();
                       w1.ShowDialog();
                       w.Close();
                   }
               });
            LoadQLPPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Quản lý phim";
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                if (p != null)
                    p.Content = new MovieManagementWindow();
            });
            LoadSuatChieuPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Quản lý suất chiếu";
                if (p != null)
                    p.Content = new ShowtimeManagement();
            });
            LoadQLNVPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Quản lý nhân sự";
                if (p != null)
                    p.Content = new NhanVienPage();
            });
            LoadQLKHPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Quản lý khách hàng";
                if (p != null)
                    p.Content = new CustomerManagement();
            });
            LoadLSPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Lịch sử";
                if (p != null)
                    p.Content = new Import_Export();
            });
            LoadTKPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Thống kê";
                if (p != null)
                    p.Content = new StatisticalManagement();
            });
            LoadFoodPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Quản lý sản phẩm";
                if (p != null)
                    p.Content = new FoodPage();

            });
            LoadErrorPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Sự cố";
                if (p != null)
                    p.Content = new ErrorManagement();

            });
            LoadVCPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (MainAdminWindow.Slidebtn != null)
                    MainAdminWindow.Slidebtn.IsChecked = false;
                SelectedFuncName = "Voucher";
                if (p != null)
                    p.Content = new VoucherManagement();

            });
            ChangeRoleCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                MainStaffWindow w1 = new MainStaffWindow();
                MainStaffViewModel.CurrentStaff = currentStaff;
                w1._StaffName.Text = currentStaff.Name;
                w1.Show();
                p.Close();
            });


            // this is  the ErrorViewmodel resources
            LoadDetailErrorCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChoseWindow();
            });
            UpdateErrorCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
             {
                 if (SelectedStatus is null)
                 {
                     MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Không hợp lệ!", MessageType.Warning, MessageButtons.OK);
                     mb.ShowDialog();
                     return;
                 }
                 IsSaving = true;
                 await UpdateErrorFunc(p);
                 IsSaving = false;
             });
            ReloadErrorListCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
             {
                 ListError = new System.Collections.ObjectModel.ObservableCollection<TroubleDTO>();
                 IsGettingSource = true;

                 await ReloadErrorList();

                 IsGettingSource = false;
             });
            SelectedDate = DateTime.Today;
            SelectedFinishDate = DateTime.Today;

            //======================================


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
        public async Task CountErrorFunc()
        {
            int counttemp = await TroubleService.Ins.GetWaitingTroubleCount();
            ErrorCount = counttemp.ToString();
        }
    }
}
