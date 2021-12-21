using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff;
using CinemaManagement.Views.Staff.MovieScheduleWindow;
using CinemaManagement.Views.Staff.OrderFoodWindow;
using CinemaManagement.Views.Staff.TicketWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.TicketBillVM
{
    public partial class TicketBillViewModel : BaseViewModel
    {
        public class Food
        {
            public string DisplayName { get; set; }
            public decimal Price { get; set; }
            public string PriceStr
            {
                get
                {
                    return Helper.FormatVNMoney(Price);
                }
            }
            public decimal TotalPrice
            {
                get
                {
                    return Price * Quantity;
                }
            }
            public string TotalPriceStr
            {
                get
                {
                    return Helper.FormatVNMoney(TotalPrice);
                }
            }
            public int Quantity { get; set; }
        }

        public static ShowtimeDTO Showtime;
        public static MovieDTO Movie;
        public static ObservableCollection<ProductDTO> ListFood;
        public static StaffDTO Staff;
        public static bool IsBacking;
        public CustomerDTO customerDTO;
        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }


        #region Biến Binding

        #region Bool display

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

        private bool _ShowDoneButton;
        public bool ShowDoneButton
        {
            get { return _ShowDoneButton; }
            set { _ShowDoneButton = value; OnPropertyChanged(); }
        }

        #endregion

        #region Value display

        private string _MovieName;
        public string MovieName
        {
            get { return _MovieName; }
            set { _MovieName = value; OnPropertyChanged(); }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(); }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged(); }
        }

        private string _Room;
        public string Room
        {
            get { return _Room; }
            set { _Room = value; OnPropertyChanged(); }
        }

        private string _Seat;
        public string Seat
        {
            get { return _Seat; }
            set { _Seat = value; OnPropertyChanged(); }
        }

        private string _Price;
        public string Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(); }
        }

        private string _TotalPriceMovie;
        public string TotalPriceMovie
        {
            get { return _TotalPriceMovie; }
            set { _TotalPriceMovie = value; OnPropertyChanged(); }
        }

        private string _TotalPriceFood;
        public string TotalPriceFood
        {
            get { return _TotalPriceFood; }
            set { _TotalPriceFood = value; OnPropertyChanged(); }
        }

        private string _TotalPrice;
        public string TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; OnPropertyChanged(); }
        }

        private string _DiscountStr;
        public string DiscountStr
        {
            get { return _DiscountStr; }
            set { _DiscountStr = value; OnPropertyChanged(); }
        }

        private string _LastPriceStr;
        public string LastPriceStr
        {
            get { return _LastPriceStr; }
            set { _LastPriceStr = value; OnPropertyChanged(); }
        }

        #endregion

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; OnPropertyChanged(); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }

        private string _NameSignUp;
        public string NameSignUp
        {
            get { return _NameSignUp; }
            set { _NameSignUp = value; OnPropertyChanged(); }
        }

        private string _EmailSignUp;
        public string EmailSignUp
        {
            get { return _EmailSignUp; }
            set { _EmailSignUp = value; OnPropertyChanged(); }
        }

        private string _VoucherID;
        public string VoucherID
        {
            get { return _VoucherID; }
            set { _VoucherID = value; OnPropertyChanged(); }
        }

        #endregion

        #region Command

        public ICommand CboxWalkinGuestCM { get; set; }
        public ICommand CheckPhoneNumberCM { get; set; }

        public ICommand OpenSignUpCM { get; set; }
        public ICommand SignUpCM { get; set; }

        public ICommand AddVoucherCM { get; set; }
        public ICommand AddVoucherNoFoodCM { get; set; }
        public ICommand AddVoucherOnlyFoodCM { get; set; }
        public ICommand DeleteVoucherCM { get; set; }

        public ICommand PayFullCM { get; set; }
        public ICommand PayMovieCM { get; set; }
        public ICommand PayFoodCM { get; set; }

        public ICommand BackToFoodPageCM { get; set; }

        #endregion

        private static List<SeatSettingDTO> _ListSeat;
        public static List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; }
        }

        private ObservableCollection<Food> _ListFoodDisplay;
        public ObservableCollection<Food> ListFoodDisplay
        {
            get => _ListFoodDisplay;
            set
            {
                _ListFoodDisplay = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<VoucherDTO> _ListVoucher;
        public ObservableCollection<VoucherDTO> ListVoucher
        {
            get => _ListVoucher;
            set
            {
                _ListVoucher = value;
                OnPropertyChanged();
            }
        }

        private VoucherDTO _SelectedItem;
        public VoucherDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public decimal LastPrice;

        DateTime start, end;
        public void CaculateTime()
        {
            start = Showtime.ShowDate;
            start = start.Add(Showtime.StartTime);
            end = start.AddMinutes(Movie.RunningTime);
        }

        public TicketBillViewModel()
        {

            #region Binding bill

            // Biến khởi tạo
            IsBacking = false;
            customerDTO = new CustomerDTO();
            ListVoucher = new ObservableCollection<VoucherDTO>();

            decimal TotalFullMoviePrice = 0;
            // Food
            ListFood = OrderFoodPageViewModel.ListOrder;
            ListFoodDisplay = new ObservableCollection<Food>();
            for (int i = 0; i < ListFood.Count; i++)
            {
                Food tempFood = new Food();
                tempFood.DisplayName = ListFood[i].DisplayName;
                tempFood.Quantity = ListFood[i].Quantity;
                tempFood.Price = ListFood[i].Price;
                ListFoodDisplay.Add(tempFood);
            }
            decimal TotalFood = 0;
            for (int i = 0; i < ListFoodDisplay.Count; i++)
            {
                TotalFood += ListFoodDisplay[i].TotalPrice;
            }
            TotalPriceFood = Helper.FormatVNMoney(TotalFood);

            bool IsBookMovie = false;

            // Movie
            if (OrderFoodPageViewModel.checkOnlyFoodOfPage == false)
            {
                Movie = TicketWindowViewModel.tempFilmName;
                Showtime = TicketWindowViewModel.CurrentShowtime;
                ListSeat = TicketWindowViewModel.WaitingList;
                MovieName = Movie.DisplayName;
                Date = Showtime.ShowDate.ToString("dd/MM/yyyy");
                CaculateTime();
                Time = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
                Seat = ListSeat[0].SeatPosition;
                for (int i = 1; i < ListSeat.Count; i++)
                {
                    Seat += ", " + ListSeat[i].SeatPosition;
                }
                Room = "0" + Showtime.RoomId.ToString();
                Price = Helper.FormatVNMoney(Showtime.TicketPrice);
                TotalPriceMovie = Helper.FormatVNMoney(Showtime.TicketPrice * ListSeat.Count);
                TotalFullMoviePrice = Showtime.TicketPrice * ListSeat.Count;
                IsBookMovie = true;
            }

            //Total price
            decimal Total = TotalFood + TotalFullMoviePrice;
            TotalPrice = Helper.FormatVNMoney(Total);

            //Discount
            decimal Discount = 0;
            DiscountStr = Helper.FormatVNMoney(Discount);
            if (ListFood.Count == 0)
            {
                LastPriceStr = TotalPriceMovie;
                LastPrice = TotalFullMoviePrice;
            }
            else if (IsBookMovie)
            {
                LastPriceStr = TotalPrice;
                LastPrice = Total;
            }
            else
            {
                LastPriceStr = TotalPriceFood;
                LastPrice = TotalFood;
            }

            #endregion

            // Display bool
            IsValidPhone = false;
            IsWalkinGuest = false;
            ShowPhone = true;
            ShowPhoneError = false;
            ShowSignUp = false;
            ShowDoneButton = false;

            CboxWalkinGuestCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    bool x = IsWalkinGuest;
                    if (IsWalkinGuest)
                    {
                        ShowPhone = false;
                        ShowDoneButton = true;
                    }
                    else
                    {
                        ShowPhone = true;
                        ShowDoneButton = false;
                    }
                    ShowPhoneError = false;
                    ShowSignUp = false;
                    ShowInfoCustomer = false;
                    PhoneNumber = "";
                    NameSignUp = "";
                    EmailSignUp = "";
                });

            CheckPhoneNumberCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    if (!string.IsNullOrEmpty(PhoneNumber))
                    {
                        CustomerDTO customer = await CustomerService.Ins.FindCustomerInfo(PhoneNumber);
                        if (customer != null)
                        {
                            Name = customer.Name;
                            Email = customer.Email;
                            IsValidPhone = true;
                            customerDTO = customer;
                        }
                        else
                        {
                            IsValidPhone = false;
                        }

                        if (IsValidPhone)
                        {
                            ShowPhoneError = false;
                            ShowSignUp = false;
                            ShowInfoCustomer = true;
                            ShowDoneButton = true;
                        }
                        else
                        {
                            if (Helper.IsPhoneNumber(PhoneNumber))
                            {
                                ShowPhoneError = true;
                                ShowInfoCustomer = false;
                                ShowDoneButton = false;
                                NameSignUp = "";
                                EmailSignUp = "";
                            }
                            else
                            {
                                MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Số điện thoại không hợp lệ", MessageType.Error, MessageButtons.OK);
                                mess.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cảnh báo", "Số điện thoại không được để trống", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    }
                    if (ListVoucher != null)
                    {
                        ListVoucher.Clear();
                    }

                });

            OpenSignUpCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ShowSignUp = true;

                });

            SignUpCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    CustomerDTO customer = new CustomerDTO();
                    if (!string.IsNullOrEmpty(PhoneNumber))
                    {
                        if (Helper.IsPhoneNumber(PhoneNumber))
                        {
                            if (!string.IsNullOrEmpty(NameSignUp))
                            {
                                if (string.IsNullOrEmpty(EmailSignUp))
                                {
                                    customer.PhoneNumber = PhoneNumber;
                                    customer.Name = NameSignUp;
                                    IsSaving = true; 

                                    (bool successAddCustomer, string messageFromAddCustomer, string newCustomer) = await CustomerService.Ins.CreateNewCustomer(customer);
                                    IsSaving = false;


                                    if (successAddCustomer)
                                    {
                                        MessageBoxCustom mgb = new MessageBoxCustom("Thông báo", messageFromAddCustomer, MessageType.Success, MessageButtons.OK);
                                        mgb.ShowDialog();
                                        NameSignUp = "";
                                        EmailSignUp = "";
                                        UpdateAddCustomer();
                                    }
                                    else
                                    {
                                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", messageFromAddCustomer, MessageType.Error, MessageButtons.OK);
                                        mess.ShowDialog();
                                    }
                                }
                                else
                                {
                                    if (RegexUtilities.IsValidEmail(EmailSignUp))
                                    {
                                        customer.PhoneNumber = PhoneNumber;
                                        customer.Name = NameSignUp;
                                        customer.Email = EmailSignUp;
                                        (bool successAddCustomer, string messageFromAddCustomer, string newCustomer) = await CustomerService.Ins.CreateNewCustomer(customer);
                                        if (successAddCustomer)
                                        {
                                            MessageBoxCustom mgb = new MessageBoxCustom("Thông báo", messageFromAddCustomer, MessageType.Success, MessageButtons.OK);
                                            mgb.ShowDialog();
                                            NameSignUp = "";
                                            EmailSignUp = "";
                                            UpdateAddCustomer();
                                        }
                                        else
                                        {
                                            MessageBoxCustom mess = new MessageBoxCustom("Lỗi", messageFromAddCustomer, MessageType.Error, MessageButtons.OK);
                                            mess.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Email không hợp lệ", MessageType.Error, MessageButtons.OK);
                                        mess.ShowDialog();
                                    }
                                }
                            }
                            else
                            {
                                new MessageBoxCustom("Cảnh báo", "Vui lòng nhập họ và tên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                            }
                        }
                        else
                        {
                            MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Số điện thoại không hợp lệ", MessageType.Error, MessageButtons.OK);
                            mess.ShowDialog();
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cảnh báo", "Vui lòng nhập số điện thoại", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    }

             });

            AddVoucherCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    if (!string.IsNullOrEmpty(VoucherID))
                    {
                        (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                        if (error == null)
                        {
                            if (ListVoucher.Count == 0)
                            {
                                if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                {
                                    ListVoucher.Add(voucher);
                                    VoucherID = "";
                                    Discount += voucher.ParValue;
                                    DiscountStr = Helper.FormatVNMoney(Discount);
                                    LastPriceStr = Helper.FormatVNMoney(Total - Discount);
                                    LastPrice = Total - Discount;
                                }
                                else
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                            }
                            else
                            {
                                if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher " + ListVoucher[0].Code + " không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                                else
                                {
                                    if (!voucher.EnableMerge)
                                    {
                                        new MessageBoxCustom("Cảnh báo", "Voucher này không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                    }
                                    else
                                    {
                                        for (int i = 0; i < ListVoucher.Count; i++)
                                        {
                                            if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                            {
                                                new MessageBoxCustom("Cảnh báo", "Voucher cùng đợt phát hành", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                                return;
                                            }
                                        }
                                        if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                        {
                                            ListVoucher.Add(voucher);
                                            VoucherID = "";
                                            Discount += voucher.ParValue;
                                            DiscountStr = Helper.FormatVNMoney(Discount);
                                            LastPriceStr = Helper.FormatVNMoney(Total - Discount);
                                            LastPrice = Total - Discount;
                                        }
                                        else
                                        {
                                            new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBoxCustom mess = new MessageBoxCustom("Lỗi", error, MessageType.Error, MessageButtons.OK);
                            mess.ShowDialog();
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cảnh báo", "Mã voucher rỗng", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    }
                });

            AddVoucherNoFoodCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    if (!string.IsNullOrEmpty(VoucherID))
                    {
                        (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                        if (error == null)
                        {
                            if (ListVoucher.Count == 0)
                            {
                                if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                {
                                    ListVoucher.Add(voucher);
                                    VoucherID = "";
                                    Discount += voucher.ParValue;
                                    DiscountStr = Helper.FormatVNMoney(Discount);
                                    LastPriceStr = Helper.FormatVNMoney(TotalFullMoviePrice - Discount);
                                    LastPrice = TotalFullMoviePrice - Discount;
                                }
                                else
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                            }
                            else
                            {
                                if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher " + ListVoucher[0].Code + " không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                                else
                                {
                                    if (voucher.ObjectType == "Sản phẩm")
                                    {
                                        new MessageBoxCustom("Cảnh báo", "Voucher không áp dụng cho phim", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                    }
                                    else
                                    {
                                        if (!voucher.EnableMerge)
                                        {
                                            new MessageBoxCustom("Cảnh báo", "Voucher này không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                        }
                                        else
                                        {
                                            for (int i = 0; i < ListVoucher.Count; i++)
                                            {
                                                if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                                {
                                                    new MessageBoxCustom("Cảnh báo", "Voucher cùng đợt phát hành", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                                    return;
                                                }
                                            }
                                            if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                            {
                                                ListVoucher.Add(voucher);
                                                VoucherID = "";
                                                Discount += voucher.ParValue;
                                                DiscountStr = Helper.FormatVNMoney(Discount);
                                                LastPriceStr = Helper.FormatVNMoney(TotalFullMoviePrice - Discount);
                                                LastPrice = TotalFullMoviePrice - Discount;
                                            }
                                            else
                                            {
                                                new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBoxCustom mess = new MessageBoxCustom("Lỗi", error, MessageType.Error, MessageButtons.OK);
                            mess.ShowDialog();
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cảnh báo", "Mã voucher rỗng", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    }
                });

            AddVoucherOnlyFoodCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    if (!string.IsNullOrEmpty(VoucherID))
                    {
                        (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                        if (error == null)
                        {
                            if (ListVoucher.Count == 0)
                            {
                                if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                {
                                    ListVoucher.Add(voucher);
                                    VoucherID = "";
                                    Discount += voucher.ParValue;
                                    DiscountStr = Helper.FormatVNMoney(Discount);
                                    LastPriceStr = Helper.FormatVNMoney(TotalFood - Discount);
                                    LastPrice = TotalFood - Discount;
                                }
                                else
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                            }
                            else
                            {
                                if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                                {
                                    new MessageBoxCustom("Cảnh báo", "Voucher " + ListVoucher[0].Code + " không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                }
                                else
                                {
                                    if (voucher.ObjectType == "Vé xem phim")
                                    {
                                        new MessageBoxCustom("Cảnh báo", "Voucher không áp dụng cho đồ ăn", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                    }
                                    else
                                    {
                                        if (!voucher.EnableMerge)
                                        {
                                            new MessageBoxCustom("Cảnh báo", "Voucher này không được dùng với các voucher khác", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                        }
                                        else
                                        {
                                            for (int i = 0; i < ListVoucher.Count; i++)
                                            {
                                                if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                                {
                                                    new MessageBoxCustom("Cảnh báo", "Voucher cùng đợt phát hành", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                                    return;
                                                }
                                            }
                                            if (voucher.VoucherInfo.MinimumOrderValue <= LastPrice)
                                            {
                                                ListVoucher.Add(voucher);
                                                VoucherID = "";
                                                Discount += voucher.ParValue;
                                                DiscountStr = Helper.FormatVNMoney(Discount);
                                                LastPriceStr = Helper.FormatVNMoney(TotalFood - Discount);
                                                LastPrice = TotalFood - Discount;
                                            }
                                            else
                                            {
                                                new MessageBoxCustom("Cảnh báo", "Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney(voucher.VoucherInfo.MinimumOrderValue) + " trở lên", MessageType.Warning, MessageButtons.OK).ShowDialog();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBoxCustom mess = new MessageBoxCustom("Lỗi", error, MessageType.Error, MessageButtons.OK);
                            mess.ShowDialog();
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cảnh báo", "Mã voucher rỗng", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    }
                });

            DeleteVoucherCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (SelectedItem != null)
                    {
                        VoucherDTO temp = SelectedItem;
                        Discount -= temp.ParValue;
                        LastPrice += temp.ParValue;
                        LastPriceStr = Helper.FormatVNMoney(LastPrice);
                        DiscountStr = Helper.FormatVNMoney(Discount);
                        ListVoucher.Remove(SelectedItem);
                    }

                });

            PayFullCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    try
                    {
                        IsSaving = true;
                        List<ProductBillInfoDTO> productBills = new List<ProductBillInfoDTO>();
                        for (int i = 0; i < ListFood.Count; i++)
                        {
                            ProductBillInfoDTO temp = new ProductBillInfoDTO();
                            temp.ProductId = ListFood[i].Id;
                            temp.Quantity = ListFood[i].Quantity;
                            temp.ProductName = ListFood[i].DisplayName;
                            temp.PricePerItem = ListFood[i].Price;
                            productBills.Add(temp);
                        }
                        List<TicketDTO> tickets = new List<TicketDTO>();
                        for (int i = 0; i < ListSeat.Count; i++)
                        {
                            TicketDTO temp = new TicketDTO();
                            temp.ShowtimeId = Showtime.Id;
                            temp.SeatId = ListSeat[i].SeatId;
                            temp.Price = Showtime.TicketPrice;
                            tickets.Add(temp);
                        }
                        BillDTO bill = new BillDTO();
                        if (!IsWalkinGuest)
                        {
                            bill.CustomerId = customerDTO.Id;
                        }
                        bill.StaffId = Staff.Id;
                        bill.TotalPrice = LastPrice;
                        bill.DiscountPrice = Discount;

                        bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                        (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateFullOptionBooking(bill, tickets, productBills);
                        if (successBooking)
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Thông báo", messageFromBooking, MessageType.Success, MessageButtons.OK);
                            mgb.ShowDialog();
                            TicketWindow ticketWindow = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                            MovieScheduleWindow movieScheduleWindow = Application.Current.Windows.OfType<MovieScheduleWindow>().FirstOrDefault();
                            ticketWindow.Close();
                            movieScheduleWindow.Close();
                            MainStaffViewModel.MaskName.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Lỗi", messageFromBooking, MessageType.Error, MessageButtons.OK);
                            mgb.ShowDialog();
                        }

                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                });

            PayMovieCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    try
                    {
                        IsSaving = true;
                        List<TicketDTO> tickets = new List<TicketDTO>();
                        for (int i = 0; i < ListSeat.Count; i++)
                        {
                            TicketDTO temp = new TicketDTO();
                            temp.ShowtimeId = Showtime.Id;
                            temp.SeatId = ListSeat[i].SeatId;
                            temp.Price = Showtime.TicketPrice;
                            tickets.Add(temp);
                        }
                        BillDTO bill = new BillDTO();
                        if (!IsWalkinGuest)
                        {
                            bill.CustomerId = customerDTO.Id;
                        }
                        bill.StaffId = Staff.Id;
                        bill.TotalPrice = LastPrice;
                        bill.DiscountPrice = Discount;
                        bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                        (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateTicketBooking(bill, tickets);
                        if (successBooking)
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Thông báo", messageFromBooking, MessageType.Success, MessageButtons.OK);
                            mgb.ShowDialog();
                            TicketWindow ticketWindow = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                            MovieScheduleWindow movieScheduleWindow = Application.Current.Windows.OfType<MovieScheduleWindow>().FirstOrDefault();
                            ticketWindow.Close();
                            movieScheduleWindow.Close();
                            MainStaffViewModel.MaskName.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Lỗi", messageFromBooking, MessageType.Error, MessageButtons.OK);
                            mgb.ShowDialog();
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }

                });

            PayFoodCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    try
                    {
                        IsSaving = true;
                        List<ProductBillInfoDTO> productBills = new List<ProductBillInfoDTO>();
                        for (int i = 0; i < ListFood.Count; i++)
                        {
                            ProductBillInfoDTO temp = new ProductBillInfoDTO();
                            temp.ProductId = ListFood[i].Id;
                            temp.Quantity = ListFood[i].Quantity;
                            temp.ProductName = ListFood[i].DisplayName;
                            temp.PricePerItem = ListFood[i].Price;
                            productBills.Add(temp);
                        }
                        BillDTO bill = new BillDTO();
                        if (!IsWalkinGuest)
                        {
                            bill.CustomerId = customerDTO.Id;
                        }
                        bill.StaffId = Staff.Id;
                        bill.TotalPrice = LastPrice;
                        bill.DiscountPrice = Discount;
                        bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                        (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateProductOrder(bill, productBills);
                        if (successBooking)
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Thông báo", messageFromBooking, MessageType.Success, MessageButtons.OK);
                            mgb.ShowDialog();
                            MainStaffWindow tk = Application.Current.Windows.OfType<MainStaffWindow>().FirstOrDefault();
                            tk.mainFrame.Content = new FoodPage();
                        }
                        else
                        {
                            IsSaving = false;
                            MessageBoxCustom mgb = new MessageBoxCustom("Lỗi", messageFromBooking, MessageType.Error, MessageButtons.OK);
                            mgb.ShowDialog();
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                });

            BackToFoodPageCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    try
                    {
                        IsBacking = true;
                        if (OrderFoodPageViewModel.checkOnlyFoodOfPage)
                        {
                            MainStaffWindow tk = Application.Current.Windows.OfType<MainStaffWindow>().FirstOrDefault();
                            tk.mainFrame.Content = new FoodPage();
                        }
                        else
                        {
                            TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                            tk.TicketBookingFrame.Content = new FoodPage();
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }

                });
        }

        public async void UpdateAddCustomer()
        {
            CustomerDTO customer = await CustomerService.Ins.FindCustomerInfo(PhoneNumber);
            Name = customer.Name;
            Email = customer.Email;
            ShowPhoneError = false;
            ShowSignUp = false;
            ShowInfoCustomer = true;
            ShowDoneButton = true;
            customerDTO = customer;
        }

    }
}
