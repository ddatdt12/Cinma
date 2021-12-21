using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.VoucherManagement;
using CinemaManagement.Views.Admin.VoucherManagement.AddVoucher;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using CinemaManagement.Views.Admin.VoucherManagement.Infor_EditWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
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
        public ICommand RefreshEmailListCM { get; set; }

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
                 catch (System.Data.Entity.Core.EntityException e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
             });


            LoadAddWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    AddVoucherWindow w = new AddVoucherWindow();
                    Unlock = false;
                    ShadowMask.Visibility = Visibility.Visible;
                    w.ShowDialog();
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    MessageBoxCustom errorMessage = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    errorMessage.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBoxCustom errorMessage = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    errorMessage.ShowDialog();
                }

            });
            LoadAddInforCM = new RelayCommand<Card>((p) =>
            {
                if (Unlock == false) return false;
                else
                    return true;
            },
            (p) =>
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

                AddVoucher w = new AddVoucher();
                GetVoucherList();
                if (SelectedItem.Status == false)
                {
                    w.releasebtn.Visibility = Visibility.Collapsed;
                    w.releasebtn2.Visibility = Visibility.Collapsed;
                }

                else
                {
                    w.releasebtn.Visibility = Visibility.Visible;
                    w.releasebtn2.Visibility = Visibility.Visible;
                }

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
                        MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Mã đã bị trùng!", MessageType.Warning, MessageButtons.OK);
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
                 try
                 {

                     Infor_EditWindow w = new Infor_EditWindow();
                     ShadowMask.Visibility = Visibility.Visible;
                     IsReleaseVoucherLoading = true;
                     await Task.Run(async () =>
                     {
                         (VoucherReleaseDTO voucherRelease, bool haveAny) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);
                         SelectedItem = voucherRelease;

                         storeAllMini = new ObservableCollection<VoucherDTO>(voucherRelease.Vouchers);
                     });
                     IsReleaseVoucherLoading = false;

                     w.ShowDialog();
                 }
                 catch (System.Data.Entity.Core.EntityException e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }

             });
            LoadInforCM = new RelayCommand<Card>((p) =>
            {
                if (Unlock == false) return false;
                else
                    return true;
            },
            (p) =>
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
                 if (SelectedItem.UnusedVCount != SelectedItem.VCount)
                 {
                     MessageBoxCustom result = new MessageBoxCustom("Không thể xoá", "Tồn tại voucher đã phát hành", MessageType.Error, MessageButtons.OK);
                     result.ShowDialog();
                 }

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
                             MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromDelete, MessageType.Success, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                         else
                         {
                             MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromDelete, MessageType.Error, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                     }
                 }
             });
            SaveNewBigVoucherCM = new RelayCommand<object>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await SaveNewBigVoucherFunc();
                IsSaving = false;
            });

            SaveMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 await SaveMiniVoucherFunc();
             });
            SaveListMiniVoucherCM = new RelayCommand<Button>((p) => { return true; }, async (p) =>
            {
                string oldstring = p.Content.ToString();

                p.Content = "";
                p.IsHitTestVisible = false;
                await SaveListMiniVoucherFunc();

                p.Content = oldstring;
                p.IsHitTestVisible = true;
            });
            UpdateBigVoucherCM = new RelayCommand<object>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateBigVoucherFunc();
                IsSaving = false;
            });
            DeleteMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await DeleteMiniVoucherFunc();
            });
            OpenReleaseVoucherCM = new RelayCommand<Button>((p) => { if (HaveUsed) return false; return true; ; }, (p) =>
            {
                ReleaseVoucher w = new ReleaseVoucher();
                w.ShowDialog();
            });
            ReleaseVoucherCM = new RelayCommand<ReleaseVoucher>((p) =>
            {
                if (IsReleaseVoucherLoading)
                {
                    return false;
                }
                return true;
            }, async
            (p) =>
            {
                IsReleaseVoucherLoading = true;

                await ReleaseVoucherFunc(p);

                IsReleaseVoucherLoading = false;

            });
            StoreWaitingListCM = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                int voucherId = int.Parse(p.Content.ToString());

                if (WaitingMiniVoucher.Contains(voucherId))
                {
                    WaitingMiniVoucher.Remove(voucherId);
                    VoucherDTO item = StoreAllMini.First(v => v.Id == voucherId);
                    item.IsChecked = false;
                }
                else
                {
                    WaitingMiniVoucher.Add(voucherId);
                    VoucherDTO item = StoreAllMini.First(v => v.Id == voucherId);
                    item.IsChecked = true;
                }
                NumberSelected = WaitingMiniVoucher.Count;
                if (!storeAllMini.Any(v => v.IsChecked))
                {
                    AddVoucher.topcheck.IsChecked = false;
                }

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
            MoreEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                foreach (var item in ListCustomerEmail)
                {
                    if (!RegexUtilities.IsValidEmail(item.Email))
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Email không hợp lệ!", MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

                for (int i = ListCustomerEmail.Count - 2; i >= 0; i--)
                {
                    if (ListCustomerEmail[ListCustomerEmail.Count - 1].Email == ListCustomerEmail[i].Email)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Email đã bị trùng!", MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

                ListCustomerEmail.Add(new CustomerEmail
                {
                    Email = "",
                    IsReadonly = false,
                    IsEnable = true
                });
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));

            });
            LessEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListCustomerEmail.RemoveAt(selectedWaitingVoucher);
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
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
                WaitingMiniVoucher.Clear();
                foreach (var item in storeAllMini)
                    item.IsChecked = false;
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                ShadowMask = p;
            });
            RefreshEmailListCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await RefreshEmailList();
            });
            ReleaseVoucherExcelCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (WaitingMiniVoucher.Count == 0)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Danh sách voucher đang trống!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }

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
                                Code = StoreAllMini[j].Code,
                                Status = StoreAllMini[j].Status
                            };
                            ReleaseVoucherList.Add(temp);
                            break;
                        }
                    }
                }
                foreach (var item in ReleaseVoucherList)
                {
                    if (item.Status == "Đã sử dụng")
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Tồn tại voucher đã sử dụng!", MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }
                IsReleaseVoucherLoading = true;

                await ExportVoucherFunc();

                IsReleaseVoucherLoading = false;

                if (IsExport)
                {
                    (bool release, string message) = await VoucherService.Ins.ReleaseMultiVoucher(WaitingMiniVoucher);

                    if (release)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Success, MessageButtons.OK);
                        mb.ShowDialog();
                        WaitingMiniVoucher.Clear();
                        try
                        {
                            (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);

                            SelectedItem = voucherReleaseDetail;
                            ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                            StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                            AddVoucher.topcheck.IsChecked = false;
                            AddVoucher._cbb.SelectedIndex = 0;
                            NumberSelected = 0;
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            MessageBoxCustom m = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                            m.ShowDialog();
                        }
                        catch (Exception)
                        {
                            MessageBoxCustom m = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                            m.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("Lỗi", message, MessageType.Error, MessageButtons.OK);
                        mb.ShowDialog();
                    }
                    return;
                }
            });
            CalculateNumberOfVoucherCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ComboBoxItem selectedNum = (ComboBoxItem)p.SelectedItem;
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(selectedNum.Content.ToString())));
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
