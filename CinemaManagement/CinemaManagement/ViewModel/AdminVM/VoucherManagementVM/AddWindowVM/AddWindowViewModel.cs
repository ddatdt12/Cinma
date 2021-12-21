using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public static bool Unlock = false;
        public Button Unlockbtn { get; set; }

        #region binding data and save to database here
        private string _ReleaseName;
        public string ReleaseName
        {
            get { return _ReleaseName; }
            set { _ReleaseName = value; OnPropertyChanged(); }
        }

        private int parValue;
        public int ParValue
        {
            get { return parValue; }
            set { parValue = value; OnPropertyChanged(); }
        }

        private static bool status;
        public static bool Status
        {
            get { return status; }
            set { status = value; }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged(); }
        }

        private DateTime finishDate;
        public DateTime FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; OnPropertyChanged(); }
        }

        private bool enableMerge;
        public bool EnableMerge
        {
            get { return enableMerge; }
            set { enableMerge = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _ObjectType;
        public ComboBoxItem ObjectType
        {
            get { return _ObjectType; }
            set { _ObjectType = value; OnPropertyChanged(); }
        }

        private decimal minimumValue;
        public decimal MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; OnPropertyChanged(); }
        }
        #endregion


        #region add random list voucher here
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }

        private int length;
        public int Length
        {
            get { return length; }
            set { length = value; OnPropertyChanged(); }
        }

        private string firstChar;
        public string FirstChar
        {
            get { return firstChar; }
            set { firstChar = value; OnPropertyChanged(); }
        }

        private string lastChar;
        public string LastChar
        {
            get { return lastChar; }
            set { lastChar = value; OnPropertyChanged(); }
        }
        #endregion

        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }

        private ObservableCollection<VoucherDTO> listMiniVoucher;
        public ObservableCollection<VoucherDTO> ListMiniVoucher
        {
            get { return listMiniVoucher; }
            set { listMiniVoucher = value; OnPropertyChanged(); }
        }

        private ObservableCollection<VoucherDTO> listViewVoucher;
        public ObservableCollection<VoucherDTO> ListViewVoucher
        {
            get { return listViewVoucher; }
            set { listViewVoucher = value; OnPropertyChanged(); }
        }

        private static ObservableCollection<VoucherDTO> storeAllMini;
        public static ObservableCollection<VoucherDTO> StoreAllMini
        {
            get { return storeAllMini; }
            set { storeAllMini = value; }
        }


        private int selectedWaitingVoucher;
        public int SelectedWaitingVoucher
        {
            get { return selectedWaitingVoucher; }
            set { selectedWaitingVoucher = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedCbbFilter;
        public ComboBoxItem SelectedCbbFilter
        {
            get { return _SelectedCbbFilter; }
            set
            {
                _SelectedCbbFilter = value; OnPropertyChanged();
                ChangeListViewSource();
            }
        }

        public ICommand LoadAddWindowCM { get; set; }
        public ICommand LoadAddInforCM { get; set; }
        public ICommand LoadAddVoucherCM { get; set; }
        public ICommand LoadAddMiniVoucherCM { get; set; }
        public ICommand LoadAddListMiniVoucherCM { get; set; }
        public ICommand MoreVoucherCM { get; set; }
        public ICommand LessVoucherCM { get; set; }
        public ICommand SaveNewBigVoucherCM { get; set; }
        public ICommand SaveUnlockBtnCM { get; set; }
        public ICommand SaveMiniVoucherCM { get; set; }
        public ICommand SaveListMiniVoucherCM { get; set; }

        public void LessVoucherFunc()
        {
            ListMiniVoucher.RemoveAt(selectedWaitingVoucher);
        }
        public async Task SaveNewBigVoucherFunc()
        {
            if (string.IsNullOrEmpty(ReleaseName))
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đủ thông tin", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }
            if (ParValue >= MinimumValue)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Mệnh giá voucher phải bé hơn tổng tối thiểu", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }
            if (StartDate > FinishDate)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Ngày hiệu lực không hợp lệ", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }
            VoucherReleaseDTO vr = new VoucherReleaseDTO
            {
                StartDate = StartDate,
                FinishDate = FinishDate,
                EnableMerge = EnableMerge,
                MinimumOrderValue = MinimumValue,
                ObjectType = ObjectType.Content.ToString(),
                ParValue = ParValue,
                ReleaseName = ReleaseName,
                StaffId = MainAdminViewModel.currentStaff.Id,
                Status = Status,
            };

            (bool isSucess, string addSuccess, VoucherReleaseDTO newVoucherRelease) = await VoucherService.Ins.CreateVoucherRelease(vr);

            if (isSucess)
            {
                Unlock = true;
                MessageBoxCustom mb = new MessageBoxCustom("Thông báo", addSuccess, MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                ListBigVoucher.Add(newVoucherRelease);
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(newVoucherRelease.Id);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom m = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    m.ShowDialog();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom m = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    m.ShowDialog();
                }

            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", addSuccess, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
        public async Task SaveMiniVoucherFunc()
        {
            foreach (VoucherDTO item in ListMiniVoucher)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Không được để trống!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
            }
            for (int i = ListMiniVoucher.Count - 2; i >= 0; i--)
            {
                if (ListMiniVoucher[ListMiniVoucher.Count - 1].Code == ListMiniVoucher[i].Code)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Mã đã bị trùng!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            (bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = await VoucherService.Ins.CreateInputVoucherList(SelectedItem.Id, new List<VoucherDTO>(ListMiniVoucher));

            if (createSuccess)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Thông báo", createRandomSuccess, MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();

                ListViewVoucher = new ObservableCollection<VoucherDTO>(ListViewVoucher.Concat(newListCode));
                SelectedItem.Vouchers = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);

                for (int i = 0; i < ListBigVoucher.Count; i++)
                {
                    if (ListBigVoucher[i].Id == selectedItem.Id)
                    {
                        VoucherReleaseDTO clone = new VoucherReleaseDTO()
                        {
                            Id = selectedItem.Id,
                            ReleaseName = selectedItem.ReleaseName,
                            StartDate = selectedItem.StartDate,
                            FinishDate = selectedItem.FinishDate,
                            MinimumOrderValue = selectedItem.MinimumOrderValue,
                            ParValue = selectedItem.ParValue,
                            ObjectType = selectedItem.ObjectType,
                            Status = selectedItem.Status,
                            StaffId = selectedItem.StaffId,
                            StaffName = selectedItem.StaffName,
                            VCount = ListViewVoucher.Count,
                            UnusedVCount = SelectedItem.UnusedVCount + newListCode.Count,
                        };
                        ListBigVoucher[i] = clone;
                        SelectedItem = ListBigVoucher[i];
                        return;
                    }
                }

                AddVoucher.topcheck.IsChecked = false;
                AddVoucher._cbb.SelectedIndex = 0;
                NumberSelected = 0;


            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", createRandomSuccess, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
        public async Task SaveListMiniVoucherFunc()
        {
            if (Quantity == 0 || Length == 0 || string.IsNullOrEmpty(FirstChar) || string.IsNullOrEmpty(LastChar))
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Không được để trống!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }

            (string error, List<string> listCode) = await Task<(string, List<string>)>.Run(() => Helper.GetListCode(Quantity, Length, FirstChar, LastChar));
            if (error != null)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", error, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }

            IsReleaseVoucherLoading = true;
            (bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = await Task<(bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode)>.Run(() => VoucherService.Ins.CreateRandomVoucherList(SelectedItem.Id, listCode));
            IsReleaseVoucherLoading = false;

            if (createSuccess)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Thông báo", createRandomSuccess, MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();

                ListViewVoucher = new ObservableCollection<VoucherDTO>(ListViewVoucher.Concat(newListCode));

                SelectedItem.Vouchers = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);

                for (int i = 0; i < ListBigVoucher.Count; i++)
                {
                    if (ListBigVoucher[i].Id == selectedItem.Id)
                    {
                        VoucherReleaseDTO clone = new VoucherReleaseDTO()
                        {
                            Id = selectedItem.Id,
                            ReleaseName = selectedItem.ReleaseName,
                            StartDate = selectedItem.StartDate,
                            FinishDate = selectedItem.FinishDate,
                            MinimumOrderValue = selectedItem.MinimumOrderValue,
                            ParValue = selectedItem.ParValue,
                            ObjectType = selectedItem.ObjectType,
                            Status = selectedItem.Status,
                            StaffId = selectedItem.StaffId,
                            StaffName = selectedItem.StaffName,
                            VCount = ListViewVoucher.Count,
                            UnusedVCount = SelectedItem.UnusedVCount + newListCode.Count,
                        };
                        ListBigVoucher[i] = clone;
                        SelectedItem = ListBigVoucher[i];
                        return;
                    }
                }

                AddVoucher.topcheck.IsChecked = false;
                AddVoucher._cbb.SelectedIndex = 0;
                NumberSelected = 0;
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", createRandomSuccess, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
        }

        public void ChangeListViewSource()
        {
            if (SelectedCbbFilter is null) return;

            NumberSelected = 0;
            if (WaitingMiniVoucher != null)
                WaitingMiniVoucher.Clear();

            ListViewVoucher = new ObservableCollection<VoucherDTO>();

            if (SelectedCbbFilter.Content.ToString() == "Toàn bộ")
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
            }
            else
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini.Where(v => v.Status == SelectedCbbFilter.Tag.ToString()).ToList());
            }
        }
        public void GetVoucherList()
        {
            if (SelectedCbbFilter is null) return;

            ListViewVoucher = new ObservableCollection<VoucherDTO>();
            try
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
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
        }
    }
}
