using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.QuanLySuatChieuPage;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLySuatChieuPageVM
{
    public partial class QuanLySuatChieuPageViewModel : BaseViewModel
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



      

        public ICommand SaveCM { get; set; }


        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
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




        public QuanLySuatChieuPageViewModel()
        {
            LoadCurrentDate();
            List<MovieDTO> movieDTOs;
            movieDTOs = MovieService.Ins.GetAllMovie();
            MovieList = new List<MovieDTO>(movieDTOs);

            LoadAddSuatChieuWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddSuatChieuWindow temp = new AddSuatChieuWindow();
                temp.ShowDialog();
            });
            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MessageBox.Show("saveee");
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

    }
}
