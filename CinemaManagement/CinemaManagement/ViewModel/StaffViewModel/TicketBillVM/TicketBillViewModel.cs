using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
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

        private static List<SeatSettingDTO> _ListSeat;
        public static List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; }
        }

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

        private string _VoucherID;
        public string VoucherID
        {
            get { return _VoucherID; }
            set { _VoucherID = value; OnPropertyChanged(); }
        }

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

        private CustomerDTO _SelectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        DateTime start, end;
        public void CaculateTime()
        {
            start = Showtime.ShowDate;
            start = start.Add(Showtime.StartTime);
            end = start.AddMinutes(Movie.RunningTime);
        }

        public TicketBillViewModel()
        {
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
            }



            //

            decimal Total = TotalFood + TotalFullMoviePrice;
            TotalPrice = Helper.FormatVNMoney(Total);

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

                });

            CheckPhoneNumberCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    IsValidPhone = !IsValidPhone;
                    if (!IsValidPhone)
                    {
                        ShowPhoneError = true;
                        ShowInfoCustomer = false;
                        ShowDoneButton = false;
                    }
                    else
                    {
                        ShowPhoneError = false;
                        ShowSignUp = false;
                        ShowInfoCustomer = true;
                        ShowDoneButton = true;
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
