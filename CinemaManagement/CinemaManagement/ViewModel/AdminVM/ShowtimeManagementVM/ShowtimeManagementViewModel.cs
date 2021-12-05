using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.ShowtimeManagementVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {
        public static Grid ShadowMask { get; set; }
        // this is for  binding data
        private MovieDTO _movieSelected;
        public MovieDTO movieSelected
        {
            get { return _movieSelected; }
            set { _movieSelected = value; OnPropertyChanged(); CalculateRunningTime(); }
        }

        private DateTime _showtimeDate;
        public DateTime showtimeDate
        {
            get { return _showtimeDate; }
            set { _showtimeDate = value; OnPropertyChanged(); }
        }

        private DateTime _Showtime;
        public DateTime Showtime
        {
            get { return _Showtime; }
            set
            {
                _Showtime = value; OnPropertyChanged(); CalculateRunningTime();
            }
        }

        private RoomDTO _ShowtimeRoom;
        public RoomDTO ShowtimeRoom
        {
            get { return _ShowtimeRoom; }
            set { _ShowtimeRoom = value; OnPropertyChanged(); }
        }

        private decimal _moviePrice;
        public decimal moviePrice
        {
            get { return _moviePrice; }
            set { _moviePrice = value; OnPropertyChanged(); }
        }

        // this is for  binding data




        private ObservableCollection<MovieDTO> _showtimeList; // this is  for the main listview
        public ObservableCollection<MovieDTO> ShowtimeList
        {
            get { return _showtimeList; }
            set { _showtimeList = value; OnPropertyChanged(); }
        }


        private ObservableCollection<MovieDTO> _movieList; // for adding showtime
        public ObservableCollection<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
                OnPropertyChanged();
            }
        }

        private List<RoomDTO> _ListRoom;    // for adding showtime
        public List<RoomDTO> ListRoom
        {
            get { return _ListRoom; }
            set { _ListRoom = value; OnPropertyChanged(); }
        }



        private DateTime _getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _getCurrentDate; }
            set { _getCurrentDate = value; }
        }
        private string _setCurrentDate;
        public string SetCurrentDate
        {
            get { return _setCurrentDate; }
            set { _setCurrentDate = value; }
        }


        private DateTime _SelectedDate;  //  changing the listview when select day
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; ReloadShowtimeList(-1); OnPropertyChanged(); }
        }

        private MovieDTO _selectedItem; //the item being selected
        public MovieDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private MovieDTO _oldselectedItem; //the item being selected
        public MovieDTO oldSelectedItem
        {
            get { return _oldselectedItem; }
            set { _oldselectedItem = value; OnPropertyChanged(); }
        }



        public int SelectedRoomId = -1;


        public ICommand ChangedRoomCM { get; set; }

        public ICommand LoadDeleteShowtimeCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }

        public ShowtimeManagementViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GetCurrentDate = DateTime.Today;
                SelectedDate = GetCurrentDate;
                showtimeDate = GetCurrentDate;
                ReloadShowtimeList();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                ShadowMask = p;
                ReloadShowtimeList();
            });
            LoadAddShowtimeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GenerateListRoom();
                RenewData();
                AddShowtimeWindow temp = new AddShowtimeWindow();
                MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
                ShadowMask.Visibility = Visibility.Visible;
                temp.ShowDialog();
            });
            SaveCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SaveShowtimeFunc(p);
            });
            LoadDeleteShowtimeCM = new RelayCommand<ListBox>((p) => { if (SelectedShowtime is null) return false; return true; }, (p) =>
            {
                string message = "Bạn có chắc muốn xoá suất chiếu này không? Dữ liệu không thể phục hồi sau khi xoá!";
                try
                {
                    //Kiểm tra suất chiếu đã có người đặt ghế nào chưa để có thông báo phù hợp
                    bool isShowHaveBooking = ShowtimeService.Ins.CheckShowtimeHaveBooking(SelectedShowtime.Id);
                    if (isShowHaveBooking)
                    {
                        message = $"Suất chiếu này có ghế đã được đặt.\n{message}";
                    }
                }
                catch (Exception e)
                {
                    MessageBoxCustom ms = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Warning, MessageButtons.OK);
                    ms.ShowDialog();
                }

                MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", message, MessageType.Warning, MessageButtons.YesNo);
                result.ShowDialog();
                if (result.DialogResult == true)
                {
                    (bool deleteSuccess, string messageFromDelete) = ShowtimeService.Ins.DeleteShowtime(SelectedShowtime.Id);
                    if (deleteSuccess)
                    {
                        for (int i = 0; i < ListShowtimeofMovie.Count; i++)
                        {
                            if (ListShowtimeofMovie[i].Id == SelectedShowtime.Id)
                                ListShowtimeofMovie.RemoveAt(i);
                        }
                        oldSelectedItem = SelectedItem;
                        ReloadShowtimeList(SelectedRoomId);
                        SelectedShowtime = null;
                        MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelete, MessageType.Success, MessageButtons.OK);
                        mb.ShowDialog();
                    }
                    else
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelete, MessageType.Error, MessageButtons.OK);
                        mb.ShowDialog();
                    }


                }
            });
            ChangedRoomCM = new RelayCommand<RadioButton>((p) => { return true; }, (p) =>
            {
                switch (p.Name)
                {
                    case "all":
                        {
                            SelectedRoomId = -1;
                            ReloadShowtimeList(SelectedRoomId);
                            break;
                        }
                    case "r1":
                        {
                            SelectedRoomId = 1;
                            ReloadShowtimeList(1);
                            break;
                        }
                    case "r2":
                        {
                            SelectedRoomId = 2;
                            ReloadShowtimeList(2);
                            break;
                        }
                    case "r3":
                        {
                            SelectedRoomId = 3;
                            ReloadShowtimeList(3);
                            break;
                        }
                    case "r4":
                        {
                            SelectedRoomId = 4;
                            ReloadShowtimeList(4);
                            break;
                        }
                    case "r5":
                        {
                            SelectedRoomId = 5;
                            ReloadShowtimeList(5);
                            break;
                        }
                }
            });
            LoadInfor_EditShowtime = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Infor_EditFunc();
            });
            CloseEditCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ShadowMask.Visibility = Visibility.Collapsed;
                p.Close();
                SelectedShowtime = null;
            });
            LoadSeatCM = new RelayCommand<ListBox>((p) => { return true; }, (p) =>
            {
                if (SelectedShowtime != null)
                {
                    GenerateSeat();
                    if (SelectedShowtime.TicketPrice.ToString().Length > 5)
                        moviePrice = decimal.Parse(SelectedShowtime.TicketPrice.ToString().Remove(5, 5));
                    else
                        moviePrice = decimal.Parse(SelectedShowtime.TicketPrice.ToString());
                }

            });
            EditPriceCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (SelectedShowtime is null) return;
                if (p.Content.ToString() == "Lưu") return;

                (bool IsSuccess, string message) = ShowtimeService.Ins.UpdateTicketPrice(SelectedShowtime.Id, moviePrice);

                if (IsSuccess)
                {
                    SelectedShowtime.TicketPrice = moviePrice;
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });
        }


        public void ReloadShowtimeList(int id = -1)
        {
            if (id != -1)
                ShowtimeList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetShowingMovieByDay(SelectedDate, id));
            else
                ShowtimeList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetShowingMovieByDay(SelectedDate));
        }
        public void GenerateListRoom()
        {
            ListRoom = new List<RoomDTO>();
            for (int i = 0; i <= 4; i++)
            {
                RoomDTO temp = new RoomDTO
                {
                    Id = i + 1,

                };
                ListRoom.Add(temp);
            }
        }
        public bool IsValidData()
        {
            return movieSelected != null
                && !string.IsNullOrEmpty(showtimeDate.ToString())
                && !string.IsNullOrEmpty(Showtime.ToString())
                && ShowtimeRoom != null && !(moviePrice == 0);
        }
    }
}
