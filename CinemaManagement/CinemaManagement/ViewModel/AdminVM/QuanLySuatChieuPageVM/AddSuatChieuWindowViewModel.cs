using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLySuatChieuPageVM
{
    public class AddSuatChieuWindowViewModel : BaseViewModel
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


        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
        }


        public ICommand SaveCM { get; set; }


        public AddSuatChieuWindowViewModel()
        {
            List<MovieDTO> movieDTOs;
            movieDTOs = MovieService.Ins.GetAllMovie();
            MovieList = new List<MovieDTO>(movieDTOs);

            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            
        }

    }
}
