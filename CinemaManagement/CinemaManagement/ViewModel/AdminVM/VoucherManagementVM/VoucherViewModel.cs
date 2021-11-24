using CinemaManagement.Views.Admin.VoucherManagement;
using CinemaManagement.Views.Admin.VoucherManagement.AddVoucher;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }



        public ICommand SavemainFrameNameCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadViewCM { get; set; }

        public VoucherViewModel()
        {





            LoadAddWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddVoucherWindow w = new AddVoucherWindow();
                w.ShowDialog();
            });

            LoadAddInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new AddInfor();
            });
            LoadAddVoucherCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new AddVoucher();
            });





            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new AddInfor();
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
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f0f2f5");
            ButtonView.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth2);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#fafafa");
            p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);
        }
    }
}
