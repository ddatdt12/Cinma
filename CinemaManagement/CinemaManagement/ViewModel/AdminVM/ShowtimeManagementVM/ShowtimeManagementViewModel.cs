using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.ShowtimeManagementVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {

        private string _movieName;
        public string movieName
        {
            get { return _movieName; }
            set { _movieName = value; }
        }

        private DateTime _movieDate;
        public DateTime movieDate
        {
            get { return _movieDate; }
            set { _movieDate = value; }
        }

        private DateTime _movieShowtime;
        public DateTime movieShowtime
        {
            get { return _movieShowtime; }
            set { _movieShowtime = value; }
        }

        private RoomDTO _movieRoom;
        public RoomDTO movieRoom
        {
            get { return _movieRoom; }
            set { _movieRoom = value; }
        }





        private ObservableCollection<MovieDTO> _showtimeList;
        public ObservableCollection<MovieDTO> ShowtimeList
        {
            get { return _showtimeList; }
            set { _showtimeList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<MovieDTO> _movieList;
        public ObservableCollection<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
                OnPropertyChanged();
            }
        }

        private List<RoomDTO> _ListRoom;
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

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; ReloadShowtimeList(); OnPropertyChanged(); }
        }

        private MovieDTO _selectedItem;
        public MovieDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }


        public ShowtimeManagementViewModel()
        {

            LoadCurrentDate();





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
        }




        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }
        public void RenewData()
        {
            movieName = null;
            movieDate = GetCurrentDate;
            movieRoom = new RoomDTO();
            movieShowtime = new DateTime();
        }

        //Operation is enum have 4 values { READ, UPDATE, CREATE, DELETE }
        public void LoadShowtimeListView(Operation oper = Operation.READ, MovieDTO m = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    ShowtimeList.Add(m);
                    break;
                //case Operation.READ:
                //    ShowtimeList= new ObservableCollection<ShowtimeDTO>(ShowtimeService.Ins.GetAllShowtime());
                //    break;
                case Operation.UPDATE:
                    var showtimeFound = ShowtimeList.FirstOrDefault(x => x.Id == m.Id);
                    ShowtimeList[ShowtimeList.IndexOf(showtimeFound)] = m;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < ShowtimeList.Count; i++)
                    {
                        if (ShowtimeList[i].Id == SelectedItem?.Id)
                        {
                            ShowtimeList.Remove(ShowtimeList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }


        public void ReloadShowtimeList()
        {
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
    }
}
