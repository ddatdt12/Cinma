using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public static int NumberCustomer;


        private DateTime _ReleaseDate;

        public DateTime ReleaseDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; OnPropertyChanged(); }
        }


        public ICommand DeleteWaitingReleaseCM { get; set; }
        public ICommand MoreEmailCM { get; set; }
        public ICommand LessEmailCM { get; set; }
        public ICommand OpenReleaseVoucherCM { get; set; }
        public ICommand ReleaseVoucherCM { get; set; }

        private ObservableCollection<VoucherDTO> releaseVoucherList;
        public ObservableCollection<VoucherDTO> ReleaseVoucherList
        {
            get { return releaseVoucherList; }
            set { releaseVoucherList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CustomerEmail> _ListCustomerEmail;
        public ObservableCollection<CustomerEmail> ListCustomerEmail
        {
            get { return _ListCustomerEmail; }
            set { _ListCustomerEmail = value; OnPropertyChanged(); }
        }

        public void ReleaseVoucherFunc(ReleaseVoucher p)
        {
            string mess = "Số voucher không chia hết cho khách hàng!";
            if (WaitingMiniVoucher.Count == 0)
            {
                MessageBox.Show("Danh sách voucher đang trống!");
                return;
            }

            //top 5 customer
            if (NumberCustomer == 5)
            {
                if (WaitingMiniVoucher.Count > 5)
                {
                    if (WaitingMiniVoucher.Count % 5 != 0)
                    {
                        MessageBox.Show(mess);
                        return;
                    }
                }
                else if (WaitingMiniVoucher.Count < 5)
                {
                    MessageBox.Show(mess);
                    return;
                }

            }
            // input customer mail
            else if (NumberCustomer == -1)
            {
                if (ListCustomerEmail.Count == 0)
                {
                    MessageBox.Show("Danh sách khách hàng đang trống!");
                    return;
                }
                if (WaitingMiniVoucher.Count > ListCustomerEmail.Count)
                {
                    if (WaitingMiniVoucher.Count % ListCustomerEmail.Count != 0)
                    {
                        MessageBox.Show(mess);
                        return;
                    }
                }
                else if (WaitingMiniVoucher.Count < ListCustomerEmail.Count)
                {
                    MessageBox.Show(mess);
                    return;
                }

                foreach(var item in ListCustomerEmail)
                {
                    if (string.IsNullOrEmpty(item.Email))
                    {
                        MessageBox.Show("Tồn tại email trống");
                        return;
                    }
                }
            }
            // new customer
            //code here


            (bool releaseSuccess, string messageFromRelease) = VoucherService.Ins.ReleaseMultiVoucher(WaitingMiniVoucher);

            if (releaseSuccess)
            {
                MessageBox.Show(messageFromRelease);
                WaitingMiniVoucher.Clear();
                (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);

                SelectedItem = voucherReleaseDetail;
                ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                StoreAllMini = new List<VoucherDTO>(ListViewVoucher);
                AddVoucher.topcheck.IsChecked = false;
                AddVoucher.AllCheckBox.Clear();
                AddVoucher._cbb.SelectedIndex = 0;
                p.Close();
            }
            else
            {
                MessageBox.Show(messageFromRelease);
            }

        }

    }

    public class CustomerEmail
    {
        public string Email { get; set; }
        public CustomerEmail() { }
    }
}
