using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
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




        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }



        private ObservableCollection<String> listMiniVoucher;
        public ObservableCollection<String> ListMiniVoucher
        {
            get { return listMiniVoucher; }
            set { listMiniVoucher = value; OnPropertyChanged(); }
        }


        private int selectedWaitingVoucher;
        public int SelectedWaitingVoucher
        {
            get { return selectedWaitingVoucher; }
            set { selectedWaitingVoucher = value; OnPropertyChanged(); }
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



        public void LessVoucherFunc()
        {
            ListMiniVoucher.RemoveAt(selectedWaitingVoucher);
        }
        public void SaveNewBigVoucherFunc()
        {
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
                SelectedItem = newVoucherRelease;
            }
            else
            {
                MessageBox.Show(addSuccess);
            }
        }
    }
}
