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

namespace CinemaManagement.ViewModel
{

    public partial class MainAdminViewModel : BaseViewModel
    {
        public ICommand SignoutCM { get; set; }
        public ICommand LoadQLPPageCM { get; set; }
        public ICommand LoadQLNVPageCM { get; set; }
        public ICommand LoadSuatChieuPageCM { get; set; }
        public ICommand LoadLSPage { get; set; }
        public ICommand LoadTKPageCM { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand LoadErrorPage { get; set; }
        public ICommand LoadVCPageCM { get; set; }


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
            SelectedFuncName = "Quản lý suất chiếu";
            CountErrorFunc();

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
                if (p != null)
                    p.Content = new MovieManagementWindow();
            });
            LoadSuatChieuPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Quản lý suất chiếu";
                if (p != null)
                    p.Content = new ShowtimeManagement();
            });
            LoadQLNVPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Quản lý nhân sự";
                if (p != null)
                    p.Content = new NhanVienPage();
            });
            LoadLSPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Lịch sử";
                if (p != null)
                    p.Content = new Import_Export();
            });
            LoadTKPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Thống kê";
                if (p != null)
                    p.Content = new StatisticalManagement();
            });
            LoadFoodPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Quản lý sản phẩm";
                if (p != null)
                    p.Content = new FoodPage();

            });
            LoadErrorPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Sự cố";
                if (p != null)
                    p.Content = new ErrorManagement();

            });
            LoadVCPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                SelectedFuncName = "Voucher";
                if (p != null)
                    p.Content = new VoucherManagement();

            });



            // this is  the ErrorViewmodel resources
            LoadDetailErrorCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChoseWindow();
            });
            UpdateErrorCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SelectedStatus is null)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Không hợp lệ!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
                UpdateErrorFunc(p);
            });
            SelectedDate = DateTime.Today;
            SelectedFinishDate = DateTime.Today;
            ReloadErrorList();
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
        public void CountErrorFunc()
        {
            List<TroubleDTO> countlist = new List<TroubleDTO>(TroubleService.Ins.GetAllTrouble());
            int counttemp = 0;
            ErrorCount = "0";
            foreach (var item in countlist)
            {
                if (item.Status == Utils.STATUS.WAITING)
                    counttemp++;
            }
            ErrorCount = counttemp.ToString();
        }
    }
}
