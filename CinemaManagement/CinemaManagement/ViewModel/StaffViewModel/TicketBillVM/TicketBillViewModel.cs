using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.TicketBillVM
{
    public partial class TicketBillViewModel : BaseViewModel
    {
        private bool _IsWalkinGuest;
        public bool IsWalkinGuest
        {
            get { return _IsWalkinGuest; }
            set { _IsWalkinGuest = value; OnPropertyChanged(); }
        }

        private bool _IsValidPhone;
        public bool IsValidPhone
        {
            get { return _IsValidPhone; }
            set { _IsValidPhone = value; OnPropertyChanged(); }
        }

        private bool _ShowPhone;
        public bool ShowPhone
        {
            get { return _ShowPhone; }
            set { _ShowPhone = value; OnPropertyChanged(); }
        }

        private bool _ShowPhoneError;
        public bool ShowPhoneError
        {
            get { return _ShowPhoneError; }
            set { _ShowPhoneError = value; OnPropertyChanged(); }
        }

        private bool _ShowSignUp;
        public bool ShowSignUp
        {
            get { return _ShowSignUp; }
            set { _ShowSignUp = value; OnPropertyChanged(); }
        }

        private bool _ShowInfoCustomer;
        public bool ShowInfoCustomer
        {
            get { return _ShowInfoCustomer; }
            set { _ShowInfoCustomer = value; OnPropertyChanged(); }
        }

        private string _VoucherID;
        public string VoucherID
        {
            get { return _VoucherID; }
            set { _VoucherID = value; OnPropertyChanged(); }
        }



        public ICommand CboxWalkinGuestCM { get; set; }
        public ICommand CheckPhoneNumberCM { get; set; }

        public ICommand SignUpCM { get; set; }

        public ICommand AddVoucherCM { get; set; }
        public ICommand DeleteVoucherCM { get; set; }

        private ObservableCollection<CustomerDTO> _ListVoucher;
        public ObservableCollection<CustomerDTO> ListVoucher
        {
            get => _ListVoucher;
            set
            {
                _ListVoucher = value;
                OnPropertyChanged();
            }
        }

        private CustomerDTO _SelectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public TicketBillViewModel()
        {
            ListVoucher = new ObservableCollection<CustomerDTO>();

            for(int i=0; i<10;i++)
            {
                CustomerDTO temp = new CustomerDTO();
                temp.Id = "DUONGDEPTRAI";
                temp.Name = "Miễn phí đủ mọi thứ!";
                ListVoucher.Add(temp);
                
            }

            IsValidPhone = false;
            IsWalkinGuest = false;
            ShowPhone = true;
            ShowPhoneError = false;
            ShowSignUp = false;

            CboxWalkinGuestCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    bool x = IsWalkinGuest;
                    if (IsWalkinGuest)
                    {
                        ShowPhone = false;
                        ShowPhoneError = false;
                        ShowSignUp = false;
                        ShowInfoCustomer = false;
                    }
                    else
                    {
                        ShowPhone = true;
                        ShowPhoneError = false;
                        ShowSignUp = false;
                        ShowInfoCustomer = false;
                    }

                });

            CheckPhoneNumberCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    IsValidPhone = !IsValidPhone;
                    if (!IsValidPhone)
                    {
                        ShowPhoneError = true;
                        ShowInfoCustomer = false;
                    }
                    else
                    {
                        ShowPhoneError = false;
                        ShowSignUp = false;
                        ShowInfoCustomer = true;
                    }

                });

            SignUpCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ShowSignUp = true;

                });

            AddVoucherCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (!string.IsNullOrEmpty(VoucherID))
                    {
                        CustomerDTO temp = new CustomerDTO();
                        temp.Id = VoucherID;
                        temp.Name = "Miễn phí tất cả các thứ!";
                        temp.PhoneNumber = "0";
                        ListVoucher.Add(temp);
                        VoucherID = "";
                    }
                    else
                    {
                        MessageBox.Show("Mã voucher rỗng!");
                    }
                });

            DeleteVoucherCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ListVoucher.Remove(SelectedItem);
                });

        }

    }
}
