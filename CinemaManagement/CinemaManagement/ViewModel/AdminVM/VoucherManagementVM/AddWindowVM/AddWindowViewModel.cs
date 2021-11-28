using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public static string StaffID;
        private string bindStaffID;
        public string BindStaffID
        {
            get { return bindStaffID; }
            set { bindStaffID = value; }
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

        private static List<VoucherDTO> storeAllMini;
        public static List<VoucherDTO> StoreAllMini
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
            set { _SelectedCbbFilter = value; OnPropertyChanged(); ChangeListViewSource(); }
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
        public void SaveNewBigVoucherFunc()
        {

            if (string.IsNullOrEmpty(ReleaseName))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin");
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
                StaffId = BindStaffID,
                Status = Status,
            };

            (bool isSucess, string addSuccess, VoucherReleaseDTO newVoucherRelease) = VoucherService.Ins.CreateVoucherRelease(vr);

            if (isSucess)
            {
                Unlock = true;
                MessageBox.Show(addSuccess);
                ListBigVoucher.Add(newVoucherRelease);

                (VoucherReleaseDTO voucherReleaseDetail, _) = VoucherService.Ins.GetVoucherReleaseDetails(newVoucherRelease.Id);
                SelectedItem = voucherReleaseDetail;
                ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                StoreAllMini = new List<VoucherDTO>(ListViewVoucher);
            }
            else
            {
                MessageBox.Show(addSuccess);
            }
        }
        public void SaveMiniVoucherFunc()
        {
            foreach (VoucherDTO item in ListMiniVoucher)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    MessageBox.Show("Không được để trống!");
                    return;
                }
            }
            for (int i = ListMiniVoucher.Count - 2; i >= 0; i--)
            {
                if (ListMiniVoucher[ListMiniVoucher.Count - 1].Code == ListMiniVoucher[i].Code)
                {
                    MessageBox.Show("Mã đã bị trùng!");
                    return;
                }
            }

            (bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = VoucherService.Ins.CreateInputVoucherList(SelectedItem.Id, new List<VoucherDTO>(ListMiniVoucher));

            if (createSuccess)
            {
                MessageBox.Show(createRandomSuccess);
                (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);

                SelectedItem = voucherReleaseDetail;
                ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                StoreAllMini = new List<VoucherDTO>(ListViewVoucher);
            }
            else
            {
                MessageBox.Show(createRandomSuccess);
            }
        }
        public void SaveListMiniVoucherFunc()
        {
            if (Quantity == 0 || Length == 0 || string.IsNullOrEmpty(FirstChar) || string.IsNullOrEmpty(LastChar))
            {
                MessageBox.Show("Không được để trống!");
                return;
            }


            (string error, List<string> listCode) = Helper.GetListCode(Quantity, Length, FirstChar, LastChar);
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            (bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = VoucherService.Ins.CreateRandomVoucherList(SelectedItem.Id, listCode);

            if (createSuccess)
            {
                MessageBox.Show(createRandomSuccess);
                (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);

                SelectedItem = voucherReleaseDetail;
                ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                StoreAllMini = new List<VoucherDTO>(ListViewVoucher);
            }
            else
            {
                MessageBox.Show(createRandomSuccess);
            }

        }
        public void ChangeListViewSource()
        {
            if (SelectedCbbFilter is null) return;
            ListViewVoucher = new ObservableCollection<VoucherDTO>();
            if (SelectedCbbFilter.Content.ToString() == Utils.VOUCHER_STATUS.USED)
            {
                foreach (var item in StoreAllMini)
                {
                    if (item.Status == SelectedCbbFilter.Content.ToString())
                    {
                        ListViewVoucher.Add(item);
                    }
                }
                return;
            }
            if (SelectedCbbFilter.Content.ToString() == Utils.VOUCHER_STATUS.USED)
            {
                foreach (var item in StoreAllMini)
                {
                    if (item.Status == SelectedCbbFilter.Content.ToString())
                    {
                        ListViewVoucher.Add(item);
                    }
                }
                return;
            }
            if (SelectedCbbFilter.Content.ToString() == Utils.VOUCHER_STATUS.UNRELEASED)
            {
                foreach (var item in StoreAllMini)
                {
                    if (item.Status == SelectedCbbFilter.Content.ToString())
                    {
                        ListViewVoucher.Add(item);
                    }
                }
                return;
            }
            if (SelectedCbbFilter.Content.ToString() == "Toàn bộ")
            {
                foreach (var item in StoreAllMini)
                {
                    if (item.Status == Utils.VOUCHER_STATUS.UNRELEASED)
                    {
                        ListViewVoucher.Add(item);
                    }
                }
                return;
            }
        }
    }
}
