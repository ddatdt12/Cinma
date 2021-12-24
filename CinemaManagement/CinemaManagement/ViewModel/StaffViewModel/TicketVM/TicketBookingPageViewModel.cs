using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.Views.Staff.TicketWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.TicketVM
{
    public partial class TicketWindowViewModel : BaseViewModel
    {

        public static ShowtimeDTO CurrentShowtime;
        public static MovieDTO tempFilmName;
        public static string showTimeRoom;
        public static List<Label> listlabel = new List<Label>();

        public ICommand SelectedSeatCM { get; set; }
        public ICommand LoadStatusSeatCM { get; set; }
        public ICommand SetStatusSeatCM { get; set; }

        #region Biến binding

        private string _price;
        public string price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }

        private SeatSettingDTO _SelectedSeat;
        public SeatSettingDTO SelectedSeat
        {
            get { return _SelectedSeat; }
            set { _SelectedSeat = value; OnPropertyChanged(); }
        }

        private string _sumCurrentSeat;
        public string sumCurrentSeat
        {
            get { return _sumCurrentSeat; }
            set { _sumCurrentSeat = value; OnPropertyChanged(); }
        }

        private string _showTimeRoomNumber;
        public string showTimeRoomNumber
        {
            get { return _showTimeRoomNumber; }
            set { _showTimeRoomNumber = value; OnPropertyChanged(); }
        }

        private string _showTime;
        public string showTime
        {
            get { return _showTime; }
            set { _showTime = value; OnPropertyChanged(); }
        }

        private string _startTime;
        public string startTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
        }

        private string _endTime;
        public string endTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(); }
        }

        private string _txtFilm;
        public string txtFilm
        {
            get { return _txtFilm; }
            set { _txtFilm = value; OnPropertyChanged(); }
        }

        private string _showDateBefore;
        public string showDateBefore
        {
            get { return _showDateBefore; }
            set { _showDateBefore = value; OnPropertyChanged(); }
        }

        private string _showDateAfter;
        public string showDateAfter
        {
            get { return _showDateAfter; }
            set { _showDateAfter = value; OnPropertyChanged(); }
        }

        private ImageSource _imgSourceFilmName;
        public ImageSource imgSourceFilmName
        {
            get { return _imgSourceFilmName; }
            set { _imgSourceFilmName = value; OnPropertyChanged(); }
        }

        private List<SeatSettingDTO> _ListSeat;
        public List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; OnPropertyChanged(); }
        }

        private List<SeatSettingDTO> _ListStatusSeat;
        public List<SeatSettingDTO> ListStatusSeat
        {
            get { return _ListStatusSeat; }
            set
            {
                _ListStatusSeat = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SeatSettingDTO> _ListSeat1;
        public ObservableCollection<SeatSettingDTO> ListSeat1
        {
            get { return _ListSeat1; }
            set { _ListSeat1 = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SeatSettingDTO> _ListSeat2;
        public ObservableCollection<SeatSettingDTO> ListSeat2
        {
            get { return _ListSeat2; }
            set { _ListSeat2 = value; OnPropertyChanged(); }
        }

        private static List<SeatSettingDTO> _WaitingList;
        public static List<SeatSettingDTO> WaitingList
        {
            get { return _WaitingList; }
            set { _WaitingList = value; }
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; OnPropertyChanged(); }
        }

        private string _totalSeat;
        public string TotalSeat
        {
            get { return _totalSeat; }
            set { _totalSeat = value; OnPropertyChanged(); }
        }

        #endregion

        public async Task GenerateSeat()
        {

            //
            ListSeat = new List<SeatSettingDTO>(await SeatService.Ins.GetSeatsByShowtime(CurrentShowtime.Id));
            ListStatusSeat = new List<SeatSettingDTO>();
            ListSeat1 = new ObservableCollection<SeatSettingDTO>();
            ListSeat2 = new ObservableCollection<SeatSettingDTO>();
            WaitingList = new List<SeatSettingDTO>();

            if (OrderFoodPageViewModel.IsBacking)
            {
                OrderFoodPageViewModel.IsBacking = false;
                TicketBookingPage tk = new TicketBookingPage();
                tk.SeatListBox1.ItemTemplate.LoadContent();

            }

            foreach (var item in ListSeat)
            {
                if (item.SeatPosition.Length == 2 && item.SeatPosition[1] < '3')
                {
                    ListSeat2.Add(item);
                }
                else
                {
                    ListSeat1.Add(item);
                }
                if (item.Status)
                    ListStatusSeat.Add(item);
            }
        }
        public void WaitingSeatList(Label p)
        {
            string id = p.Content.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var item in WaitingList)
                {
                    if (item.SeatPosition == id)
                    {
                        WaitingList.RemoveAll(r => r.SeatPosition == id);
                        ReCalculate();
                        return;
                    }
                }
                WaitingList.Add(SelectedSeat);
                ReCalculate();
            }
        }

        public void ReCalculate(SeatSettingDTO seat = null)
        {
            decimal totalprice = 0;
            foreach (var item in WaitingList)
            {
                totalprice += CurrentShowtime.TicketPrice;
            }

            TotalPrice = Helper.FormatVNMoney(totalprice);


            TotalSeat = "";
            for (int i = 0; i < WaitingList.Count; i++)
            {
                if (WaitingList[i] is null)
                {
                    return;
                }
                if (i == 0)
                    TotalSeat += WaitingList[i].SeatPosition;
                else
                    TotalSeat += ", " + WaitingList[i].SeatPosition;
            }
        }

        public bool IsExist(string id)
        {
            foreach (var item in WaitingList)
            {
                if (item.SeatPosition == id) return true;

            }
            return false;
        }

        DateTime start, end;
        public void CaculateTime()
        {
            start = CurrentShowtime.ShowDate;
            start = start.Add(CurrentShowtime.StartTime);
            end = start.AddMinutes(tempFilmName.RunningTime);
        }

        public async void Output_ToString()
        {
            showTimeRoomNumber = showTimeRoom;
            imgSourceFilmName = await CloudinaryService .Ins.LoadImageFromURL(tempFilmName.Image);
            txtFilm = tempFilmName.DisplayName;
            startTime = CurrentShowtime.StartTime.ToString("hh\\:mm");
            endTime = end.ToString("HH:mm");
            showTime = startTime + " - " + endTime;
            showDateAfter = start.ToString("dd-MM-yyyy");
            showDateBefore = end.ToString("dd-MM-yyyy");
            price = Helper.FormatVNMoney(CurrentShowtime.TicketPrice);
        }
    }
}
