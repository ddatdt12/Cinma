using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.TicketVM
{
    class TicketWindowViewModel : BaseViewModel
    {
        public static ShowtimeDTO CurrentShowtime;
        private SeatSettingDTO _SelectedSeat;

        public SeatSettingDTO SelectedSeat
        {
            get { return _SelectedSeat; }
            set { _SelectedSeat = value; OnPropertyChanged();}
        }

        public static MovieDTO tempFilmName;

        private string _txtFilm;
        public string txtFilm
        {
            get { return _txtFilm; }
            set { _txtFilm = value; OnPropertyChanged(); }
        }

        private ImageSource _imgSourceFilmName;

        public ImageSource imgSourceFilmName
        {
            get { return _imgSourceFilmName; }
            set { _imgSourceFilmName = value; }
        }

        private List<SeatSettingDTO> _ListSeat;
        public List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; }
        }

        private List<SeatSettingDTO> _ListSeat1;
        public List<SeatSettingDTO> ListSeat1
        {
            get { return _ListSeat1; }
            set { _ListSeat1 = value; }
        }
        private List<SeatSettingDTO> _ListSeat2;
        public List<SeatSettingDTO> ListSeat2
        {
            get { return _ListSeat2; }
            set { _ListSeat2 = value; }
        }

        private List<SeatSettingDTO> _WaitingList;
        public List<SeatSettingDTO> WaitingList
        {
            get { return _WaitingList; }
            set { _WaitingList = value; }
        }

        public ICommand CloseTicketWindowCM { get; set; }
        public ICommand MinimizeTicketWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand SelectedSeatCM { get; set; }



        private int _totalPrice;
        public int TotalPrice
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



        public TicketWindowViewModel()
        {
            GenerateSeat();
            imgSourceFilmName = tempFilmName.ImgSource;
            txtFilm = tempFilmName.DisplayName;
            WaitingList = new List<SeatSettingDTO>();

            CloseTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });
            MinimizeTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.WindowState = WindowState.Minimized;
                }
            });
            MouseMoveWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            SelectedSeatCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    foreach (var item in WaitingList)
                    {
                        if (IsExist(p.Content.ToString()))
                        {
                            p.Background = new SolidColorBrush(Colors.Transparent);
                            WaitingSeatList(p.Content.ToString());
                            p.Foreground = new SolidColorBrush(Colors.Black);
                            return;
                        }
                    }
                    p.Background = new SolidColorBrush(Colors.Green);
                    p.Foreground = new SolidColorBrush(Colors.White);
                    WaitingSeatList(p.Content.ToString());
                }
            });
        }


        public void GenerateSeat()
        {
            ListSeat = SeatService.Ins.GetSeatsByShowtime(CurrentShowtime.Id);
            ListSeat1 = new List<SeatSettingDTO>();
            ListSeat2 = new List<SeatSettingDTO>();
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
            }
        }
        public void WaitingSeatList(string id)
        {
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
            TotalPrice = 0;
            foreach (var item in WaitingList)
            {
                TotalPrice += 45000;
            }
            TotalSeat = WaitingList.Count.ToString();
        }

        public bool IsExist(string id)
        {
            foreach(var item in WaitingList)
            {
                if (item.SeatPosition == id) return true;

            }
            return false;
        }
    }
}
