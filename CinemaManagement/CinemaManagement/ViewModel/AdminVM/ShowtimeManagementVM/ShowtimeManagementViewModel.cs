using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.ShowtimeManagementVM;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {
        public static Grid ShadowMask { get; set; }

        private Label resultLabel;
        public Label ResultLabel
        {
            get { return resultLabel; }
            set { resultLabel = value; OnPropertyChanged(); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        // this is for  binding data
        private MovieDTO _movieSelected;
        public MovieDTO movieSelected
        {
            get { return _movieSelected; }
            set { _movieSelected = value; OnPropertyChanged(); }
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

        private List<MovieDTO> listMovieAllRoom = new List<MovieDTO>();



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
            set { _SelectedDate = value; OnPropertyChanged(); }
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
        public ICommand CalculateRunningTimeCM { get; set; }
        public ICommand SelectedDateCM { get; set; }
        public ICommand SaveResultNameCM { get; set; }

        public ShowtimeManagementViewModel()
        {
            CalculateRunningTimeCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                CalculateRunningTime();
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                GetCurrentDate = DateTime.Today;
                SelectedDate = GetCurrentDate;
                showtimeDate = GetCurrentDate;
                await ReloadShowtimeList();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, async (p) =>
             {
                 ShadowMask = p;
                 await ReloadShowtimeList();
             });
            LoadAddShowtimeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 GenerateListRoom();
                 RenewData();
                 AddShowtimeWindow temp = new AddShowtimeWindow();

                 try
                 {
                     IsLoading = true;
                     MovieList = new ObservableCollection<MovieDTO>(await MovieService.Ins.GetAllMovie());
                     IsLoading = false;
                 }
                 catch (System.Data.Entity.Core.EntityException e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }

                 ShadowMask.Visibility = Visibility.Visible;
                 temp.ShowDialog();
             });
            SaveCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                IsSaving = true;

                await SaveShowtimeFunc(p);

                IsSaving = false;

            });
            LoadDeleteShowtimeCM = new RelayCommand<ListBox>((p) => { if (SelectedShowtime is null) return false; return true; }, async (p) =>
             {
                 string message = "Bạn có chắc muốn xoá suất chiếu này không?";

                 //Kiểm tra suất chiếu đã có người đặt ghế nào chưa để có thông báo phù hợp
                 bool isShowHaveBooking = await ShowtimeService.Ins.CheckShowtimeHaveBooking(SelectedShowtime.Id);
                 if (isShowHaveBooking)
                 {
                     message = $"Suất chiếu này có ghế đã được đặt. Bạn có muốn xoá không?";
                     MessageBoxCustom ms = new MessageBoxCustom("Cảnh báo", message, MessageType.Warning, MessageButtons.YesNo);
                     ms.ShowDialog();
                     if (ms.DialogResult == false)
                     {
                         return;
                     }
                 }
                 else
                 {
                     MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", message, MessageType.Warning, MessageButtons.YesNo);
                     result.ShowDialog();
                     if (result.DialogResult == false)
                     {
                         return;
                     }
                 }

                 int showtimeId = SelectedShowtime.Id;
                 (bool deleteSuccess, string messageFromDelete) = await ShowtimeService.Ins.DeleteShowtime(SelectedShowtime.Id);
                 if (deleteSuccess)
                 {
                     for (int i = 0; i < ListShowtimeofMovie.Count; i++)
                     {
                         if (ListShowtimeofMovie[i].Id == showtimeId)
                             ListShowtimeofMovie.RemoveAt(i);
                     }
                     oldSelectedItem = SelectedItem;
                     SelectedShowtime = null;
                     MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromDelete, MessageType.Success, MessageButtons.OK);
                     mb.ShowDialog();

                     await ReloadShowtimeList(-1);
                     GetShowingMovieByRoomInDate(SelectedRoomId);
                 }
                 else
                 {
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromDelete, MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
             });
            ChangedRoomCM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {
                switch (p.Name)
                {
                    case "all":
                        {
                            SelectedRoomId = -1;
                            GetShowingMovieByRoomInDate(SelectedRoomId);
                            break;
                        }
                    case "r1":
                        {
                            SelectedRoomId = 1;
                            await ReloadShowtimeList(1);
                            break;
                        }
                    case "r2":
                        {
                            SelectedRoomId = 2;
                            await ReloadShowtimeList(2);
                            break;
                        }
                    case "r3":
                        {
                            SelectedRoomId = 3;
                            await ReloadShowtimeList(3);
                            break;
                        }
                    case "r4":
                        {
                            SelectedRoomId = 4;
                            await ReloadShowtimeList(4);
                            break;
                        }
                    case "r5":
                        {
                            SelectedRoomId = 5;
                            await ReloadShowtimeList(5);
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
            LoadSeatCM = new RelayCommand<ListBox>((p) => { return true; }, async (p) =>
             {
                 if (SelectedShowtime != null)
                 {
                     await GenerateSeat();
                     if (SelectedShowtime.TicketPrice.ToString().Length > 5)
                         moviePrice = decimal.Parse(SelectedShowtime.TicketPrice.ToString().Remove(5, 5));
                     else
                         moviePrice = decimal.Parse(SelectedShowtime.TicketPrice.ToString());
                 }

             });
            EditPriceCM = new RelayCommand<Label>((p) => { return true; }, async (p) =>
            {
                if (SelectedShowtime is null) return;
                if (p.Content.ToString() == "Lưu") return;

                (bool IsSuccess, string message) = await ShowtimeService.Ins.UpdateTicketPrice(SelectedShowtime.Id, moviePrice);

                if (IsSuccess)
                {
                    SelectedShowtime.TicketPrice = moviePrice;
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });
            SelectedDateCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ReloadShowtimeList(-1);
                GetShowingMovieByRoomInDate(SelectedRoomId);
            });
            SaveResultNameCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                ResultLabel = p;
            });
        }


        public async Task ReloadShowtimeList(int id = -1)
        {

            if (id != -1)
            {
                try
                {
                    GetShowingMovieByRoomInDate(id);
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                try
                {
                    ShowtimeList = new ObservableCollection<MovieDTO>();
                    IsLoading = true;

                    listMovieAllRoom = await Task.Run(() => MovieService.Ins.GetShowingMovieByDay(SelectedDate));

                    ShowtimeList = new ObservableCollection<MovieDTO>(listMovieAllRoom);
                    IsLoading = false;
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            ResultLabel.Content = ShowtimeList.Count;
        }
        private void GetShowingMovieByRoomInDate(int roomId)
        {
            if (roomId == -1)
            {
                ShowtimeList = new ObservableCollection<MovieDTO>(listMovieAllRoom);
                ResultLabel.Content = ShowtimeList.Count;
                return;
            }
            List<MovieDTO> listMoviesByRoom = listMovieAllRoom.Where(m => m.Showtimes.Any(s => s.RoomId == roomId)).Select(m => new MovieDTO
            {
                Id = m.Id,
                DisplayName = m.DisplayName,
                RunningTime = m.RunningTime,
                Country = m.Country,
                Description = m.Description,
                ReleaseYear = m.ReleaseYear,
                MovieType = m.MovieType,
                Director = m.Director,
                Image = m.Image,
                Genres = new List<GenreDTO>(m.Genres),
                Showtimes = m.Showtimes.Where(s => s.RoomId == roomId).ToList()
            }).ToList();
            ShowtimeList = new ObservableCollection<MovieDTO>(listMoviesByRoom);
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
