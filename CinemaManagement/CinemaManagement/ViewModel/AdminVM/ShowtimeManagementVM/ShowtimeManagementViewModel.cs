using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.ShowtimeManagementVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {

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
            set{
                _Showtime = value; OnPropertyChanged();}
        }

        private RoomDTO _ShowtimeRoom;
        public RoomDTO ShowtimeRoom
        {
            get { return _ShowtimeRoom; }
            set { _ShowtimeRoom = value; OnPropertyChanged(); }
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
            set { _SelectedDate = value; ReloadShowtimeList(); OnPropertyChanged(); }
        }

        private MovieDTO _selectedItem; //the showtime being selected
        public MovieDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }



        public ICommand ChangedRoomCM { get; set; }
        public ICommand LoadDeleteShowtimeCM { get; set; }


        public ShowtimeManagementViewModel()
        {

            LoadCurrentDate();
            SelectedDate = GetCurrentDate;




            LoadAddShowtimeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GenerateListRoom();

                AddShowtimeWindow temp = new AddShowtimeWindow();
                MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
                temp.ShowDialog();
            });
            SaveCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SaveShowtimeFunc(p);
            });
            LoadDeleteShowtimeCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                MessageBox.Show("deleted");
            });
            ChangedRoomCM = new RelayCommand<RadioButton>((p) => { return true; }, (p) =>
            {
                switch (p.Name)
                {
                    case "All":
                        {
                            ReloadShowtimeList();
                            break;
                        }
                    case "r1":
                        {
                            ReloadShowtimeList(1);
                            break;
                        }
                    case "r2":
                        {
                            ReloadShowtimeList(2);
                            break;

                        }
                    case "r3":
                        {
                            ReloadShowtimeList(3);
                            break;
                        }
                    case "r4":
                        {
                            ReloadShowtimeList(4);
                            break;
                        }
                    case "r5":
                        {
                            ReloadShowtimeList(5);
                            break;
                        }
                }
            });
        }




        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }
        public void RenewData()
        {
            movieSelected = null;
            showtimeDate = GetCurrentDate;
            ShowtimeRoom = null;
            Showtime = new DateTime();
        }

        ////Operation is enum have 4 values { READ, UPDATE, CREATE, DELETE }
        //public void LoadShowtimeListView(Operation oper = Operation.READ, MovieDTO m = null)
        //{
        //    switch (oper)
        //    {
        //        case Operation.CREATE:
        //            ShowtimeList.Add(m);
        //            break;
        //        //case Operation.READ:
        //        //    ShowtimeList= new ObservableCollection<ShowtimeDTO>(ShowtimeService.Ins.GetAllShowtime());
        //        //    break;
        //        case Operation.UPDATE:
        //            var showtimeFound = ShowtimeList.FirstOrDefault(x => x.Id == m.Id);
        //            ShowtimeList[ShowtimeList.IndexOf(showtimeFound)] = m;
        //            break;
        //        case Operation.DELETE:
        //            for (int i = 0; i < ShowtimeList.Count; i++)
        //            {
        //                if (ShowtimeList[i].Id == SelectedItem?.Id)
        //                {
        //                    ShowtimeList.Remove(ShowtimeList[i]);
        //                    break;
        //                }
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}


        public void ReloadShowtimeList(int id = -1)
        {
            if (id == -1)
                ShowtimeList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetShowingMovieByDay(SelectedDate));
            else
                ShowtimeList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetShowingMovieByDay(SelectedDate, id));
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
                && ShowtimeRoom != null;
        }
    }
}
