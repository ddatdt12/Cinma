using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

        private ComboBoxItem _ReleaseCustomerList;

        public ComboBoxItem ReleaseCustomerList
        {
            get { return _ReleaseCustomerList; }
            set { _ReleaseCustomerList = value; RefreshEmailList(); }
        }


        public ICommand DeleteWaitingReleaseCM { get; set; }
        public ICommand MoreEmailCM { get; set; }
        public ICommand LessEmailCM { get; set; }
        public ICommand OpenReleaseVoucherCM { get; set; }
        public ICommand ReleaseVoucherCM { get; set; }
        public ICommand ResetSelectedNumberCM { get; set; }

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
            foreach (var item in ListCustomerEmail)
            {
                if (string.IsNullOrEmpty(item.Email))
                {
                    MessageBox.Show("Tồn tại email trống");
                    return;
                }
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
                NumberSelected = 0;
                p.Close();
            }
            else
            {
                MessageBox.Show(messageFromRelease);
            }

        }
        public void RefreshEmailList()
        {
            if (ReleaseCustomerList is null) return;

            switch (ReleaseCustomerList.Content.ToString())
            {
                case "Top 5 khách hàng trong tháng":
                    {
                        (List<CustomerDTO> top5cus, _, _) = StatisticsService.Ins.GetTop5CustomerExpenseByMonth(DateTime.Today.Month);
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();

                        foreach (var item in top5cus)
                        {
                            ListCustomerEmail.Add(new CustomerEmail { Email = item.Email });
                        }

                        return;
                    }
                case "Khác":
                    {
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();
                        return;
                    }
                case "Khách hàng mới trong tháng":
                    {
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();
                        return;
                    }
            }
        }
    }

    public class CustomerEmail
    {
        public string Email { get; set; }
        public CustomerEmail() { }
    }
}
