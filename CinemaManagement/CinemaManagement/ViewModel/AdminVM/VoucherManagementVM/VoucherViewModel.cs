using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.VoucherManagement;
using CinemaManagement.Views.Admin.VoucherManagement.AddVoucher;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using CinemaManagement.Views.Admin.VoucherManagement.Infor_EditWindow;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }


        private List<StaffDTO> listtemp;
        public List<StaffDTO> Listtemp
        {
            get { return listtemp; }
            set { listtemp = value; OnPropertyChanged(); }
        }

        public ICommand SavemainFrameNameCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadViewCM { get; set; }
        public ICommand LoadEdit_InforViewCM { get; set; }

        public VoucherViewModel()
        {
            GetCurrentDate = System.DateTime.Today;

            Listtemp = new List<StaffDTO>();
            Listtemp.Add(new StaffDTO
            {
                Name = "tran khoi",
            });
            Listtemp.Add(new StaffDTO
            {
                Name = "tran khoi",
            }); Listtemp.Add(new StaffDTO
            {
                Name = "tran khoi",
            }); Listtemp.Add(new StaffDTO
            {
                Name = "tran khoi",
            }); Listtemp.Add(new StaffDTO
            {
                Name = "tran khoi",
            });

            LoadAddWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddVoucherWindow w = new AddVoucherWindow();
                w.ShowDialog();
            });
            LoadAddInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                mainFrame.Content = new AddInfor();
            });
            LoadAddVoucherCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                mainFrame.Content = new AddVoucher();
            });
            LoadAddMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddMiniVoucher w = new AddMiniVoucher();
                ListMiniVoucher = new ObservableCollection<int>();
                ListMiniVoucher.Add(ListMiniVoucher.Count + 1);
                w.ShowDialog();
            });
            LoadAddListMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddListMiniVoucher w = new AddListMiniVoucher();
                w.ShowDialog();
            });
            MoreVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListMiniVoucher.Add(ListMiniVoucher.Count + 1);
            });
            LessVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LessVoucherFunc();
            });
            LoadInforBigVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Infor_EditWindow w = new Infor_EditWindow();
                w.ShowDialog();
            });
            LoadInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ChangeView(p);
                mainFrame.Content = new Edit_InforPage();
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
