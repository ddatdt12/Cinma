using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
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
        public static Grid ShadowMask { get; set; }
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }

        private bool _IsReleaseVoucherLoading;
        public bool IsReleaseVoucherLoading
        {
            get { return _IsReleaseVoucherLoading; }
            set { _IsReleaseVoucherLoading = value; OnPropertyChanged(); }
        }

        private VoucherReleaseDTO selectedItem;
        public VoucherReleaseDTO SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

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
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }

        public VoucherViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 GetCurrentDate = DateTime.Today;
                 StartDate = FinishDate = DateTime.Today;

                 try
                 {
                    //Loading UI Handler Here
                    ListBigVoucher = new ObservableCollection<VoucherReleaseDTO>(await VoucherService.Ins.GetAllVoucherReleases());
                 }
                 catch (Exception e)
                 {

                     throw e;
                 }
             });


            LoadAddWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddVoucherWindow w = new AddVoucherWindow();
                BindStaffID = StaffID;
                Unlock = false;
                ShadowMask.Visibility = Visibility.Visible;
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
            async (p) =>
            {
                if (p is null) return;
                ChangeView(p);

                AddVoucher w = new AddVoucher();
                if (SelectedItem.Status == false)
                    w.releasebtn.Visibility = Visibility.Collapsed;
                else
                    w.releasebtn.Visibility = Visibility.Visible;

                await GetVoucherReleaseDetails();
                mainFrame.Content = w;
                WaitingMiniVoucher = new List<int>();
                NumberSelected = 0;
            });
            LoadAddMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddMiniVoucher w = new AddMiniVoucher();
                ListMiniVoucher = new ObservableCollection<VoucherDTO>();
                ListMiniVoucher.Add(new VoucherDTO
                {
                    Code = "",
                    VoucherReleaseId = SelectedItem.Id
                });
                w.ShowDialog();
            });
            LoadAddListMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddListMiniVoucher w = new AddListMiniVoucher();
                w.ShowDialog();
            });
            MoreVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                for (int i = ListMiniVoucher.Count - 2; i >= 0; i--)
                {
                    if (ListMiniVoucher[ListMiniVoucher.Count - 1].Code == ListMiniVoucher[i].Code)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", "Mã đã bị trùng!", MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

                ListMiniVoucher.Add(new VoucherDTO
                {
                    Code = "",
                    VoucherReleaseId = SelectedItem.Id
                });
            });
            LessVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LessVoucherFunc();
            });
            LoadInforBigVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 if (SelectedItem is null) return;

                 (VoucherReleaseDTO vc, bool haveAny) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);
                 SelectedItem = vc;
                 Infor_EditWindow w = new Infor_EditWindow();
                 ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                 StoreAllMini = new List<VoucherDTO>(ListViewVoucher);
                 ShadowMask.Visibility = Visibility.Visible;
                 w.ShowDialog();
             });
            LoadInforCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                Edit_InforPage w = new Edit_InforPage();
                LoadEdit_InforViewDataFunc(w);
                mainFrame.Content = w;
                Unlock = true;
            });
            LoadDeleteVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 if (SelectedItem is null) return;

                 else
                 {

                     string message = "Bạn có chắc muốn xoá đợt phát hành này không? Dữ liệu không thể phục hồi sau khi xoá!";
                     MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", message, MessageType.Warning, MessageButtons.YesNo);
                     result.ShowDialog();

                     if (result.DialogResult == true)
                     {
                         (bool deleteSuccess, string messageFromDelete) = await VoucherService.Ins.DeteleVoucherRelease(SelectedItem.Id);

                         if (deleteSuccess)
                         {
                             ListBigVoucher.Remove(SelectedItem);
                             MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelete, MessageType.Success, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                         else
                         {
                             MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelete, MessageType.Error, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                     }
                 }
             });
            SaveNewBigVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await SaveNewBigVoucherFunc();
            });

            SaveMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 await SaveMiniVoucherFunc();
             });
            SaveListMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await SaveListMiniVoucherFunc();
            });
            UpdateBigVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await UpdateBigVoucherFunc();
            });
            DeleteMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await DeleteMiniVoucherFunc();
            });
            OpenReleaseVoucherCM = new RelayCommand<Button>((p) =>
            {
                if (HaveUsed)
                    return false;
                return true; ;
            },
            (p) =>
            {
                ReleaseVoucher w = new ReleaseVoucher();
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>();

                for (int i = 0; i < WaitingMiniVoucher.Count; i++)
                {
                    for (int j = 0; j < StoreAllMini.Count; j++)
                    {
                        if (WaitingMiniVoucher[i] == StoreAllMini[j].Id)
                        {
                            VoucherDTO temp = new VoucherDTO
                            {
                                Id = WaitingMiniVoucher[i],
                                Code = StoreAllMini[j].Code
                            };
                            ReleaseVoucherList.Add(temp);
                            break;
                        }
                    }
                }

                w.ShowDialog();
            });
            ReleaseVoucherCM = new RelayCommand<ReleaseVoucher>((p) => { return true; }, async (p) =>
            {
                IsReleaseVoucherLoading = true;

                await ReleaseVoucherFunc(p);

                IsReleaseVoucherLoading = false;

            });
            StoreWaitingListCM = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                int temp = int.Parse(p.Content.ToString());
                if (p.IsChecked == false)
                {
                    if (WaitingMiniVoucher.Contains(temp))
                    {
                        WaitingMiniVoucher.Remove(temp);
                        NumberSelected--;
                    }

                }
                else
                {
                    if (!WaitingMiniVoucher.Contains(temp))
                    {
                        WaitingMiniVoucher.Add(temp);
                        NumberSelected++;
                    }

                }

                if (WaitingMiniVoucher.Count == 0)
                    AddVoucher.topcheck.IsChecked = false;

            });
            CheckAllMiniVoucherCM = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                {
                    CheckAllMiniVoucherFunc(true);
                }
                else
                {
                    CheckAllMiniVoucherFunc(false);
                }

            });
            DeleteWaitingReleaseCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedWaitingVoucher >= 0)
                {
                    foreach (var item in WaitingMiniVoucher)
                    {
                        if (item == ReleaseVoucherList[SelectedWaitingVoucher].Id)
                        {
                            WaitingMiniVoucher.Remove(item);
                            break;
                        }

                    }
                    ReleaseVoucherList.RemoveAt(SelectedWaitingVoucher);
                }
            });
            MoreEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                for (int i = ListCustomerEmail.Count - 2; i >= 0; i--)
                {
                    if (ListCustomerEmail[ListCustomerEmail.Count - 1].Email == ListCustomerEmail[i].Email)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", "Email đã bị trùng!", MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

                ListCustomerEmail.Add(new CustomerEmail
                {
                    Email = "",
                });
            });
            LessEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListCustomerEmail.RemoveAt(selectedWaitingVoucher);
            });

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new AddInfor();
            });
            LoadEdit_InforViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                Edit_InforPage w = new Edit_InforPage();
                LoadEdit_InforViewDataFunc(w);
                p.Content = w;
                Unlock = true;
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
            ResetSelectedNumberCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NumberSelected = 0;
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                ShadowMask = p;
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
