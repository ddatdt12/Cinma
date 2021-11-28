using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.VoucherManagement;
using CinemaManagement.Views.Admin.VoucherManagement.AddVoucher;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using CinemaManagement.Views.Admin.VoucherManagement.Infor_EditWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }

        private VoucherReleaseDTO selectedItem;
        public VoucherReleaseDTO SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }


        //private List<VoucherReleaseDTO> _ListBigVoucher;
        //public List<VoucherReleaseDTO> ListBigVoucher
        //{
        //    get { return _ListBigVoucher; }
        //    set { _ListBigVoucher = value; OnPropertyChanged(); }
        //}
        public ObservableCollection<VoucherReleaseDTO> _ListBigVoucher;
        public ObservableCollection<VoucherReleaseDTO> ListBigVoucher
        {
            get { return _ListBigVoucher; }
            set { _ListBigVoucher = value; OnPropertyChanged(); }
        }

        public ICommand SavemainFrameNameCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadViewCM { get; set; }
        public ICommand LoadEdit_InforViewCM { get; set; }
        public ICommand LoadDeleteVoucherCM { get; set; }

        public VoucherViewModel()
        {
            GetCurrentDate = DateTime.Today;
            StartDate = FinishDate = DateTime.Today;

            try
            {
                ListBigVoucher = new ObservableCollection<VoucherReleaseDTO>(VoucherService.Ins.GetAllVoucherReleases());
            }
            catch (Exception e)
            {

                throw e;
            }


            LoadAddWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddVoucherWindow w = new AddVoucherWindow();
                BindStaffID = StaffID;
                w.ShowDialog();
            });
            LoadAddInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                AddInfor w = new AddInfor();
                mainFrame.Content = w;
            });
            LoadAddVoucherCM = new RelayCommand<Card>((p) =>
            {
                if (Unlock == false) return false;
                else
                    return true;
            }, 
            (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                mainFrame.Content = new AddVoucher();
            });
            LoadAddMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddMiniVoucher w = new AddMiniVoucher();
                ListMiniVoucher = new ObservableCollection<String>();
                ListMiniVoucher.Add("");
                w.ShowDialog();
            });
            LoadAddListMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddListMiniVoucher w = new AddListMiniVoucher();
                w.ShowDialog();
            });
            MoreVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListMiniVoucher.Add("");
            });
            LessVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LessVoucherFunc();
            });
            LoadInforBigVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem is null) return;

                Infor_EditWindow w = new Infor_EditWindow();
                w.ShowDialog();
            });
            LoadInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                mainFrame.Content = new Edit_InforPage();
            });
            LoadDeleteVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem is null) return;

                else
                    MessageBox.Show("delete");
            });
            SaveNewBigVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SaveNewBigVoucherFunc();
            });


            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new AddInfor();
            });
            LoadEdit_InforViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new Edit_InforPage();
            });
            SavemainFrameNameCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new AddInfor();
            });
            StoreButtonNameCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ButtonView = p;
                p.Background = new SolidColorBrush(Colors.White);
                p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);
            });
        }
        public void ChangeView(Card p)
        {
            if (p is null) return;
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f0f2f5");
            ButtonView.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth2);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#fafafa");
            p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);
        }
    }
}
