using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using MaterialDesignThemes.Wpf;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{
    public class QuanLyPhimPageViewModel : BaseViewModel
    {

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

        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
        }

        private MovieDTO _selectedItem;
        public MovieDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ICommand Open_AddMovieWindowCM { get; set; }
        public ICommand SelectedMovieItemCM { get; set; }
        public ICommand EditCM { get; set; }

        public QuanLyPhimPageViewModel()
        {

            LoadCurrentDate();
            List<MovieDTO> movieDTOs;
            movieDTOs = MovieService.Ins.GetAllMovie();
            MovieList = new List<MovieDTO>(movieDTOs);


            Open_AddMovieWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new AddMovieWindow();
                w1.ShowDialog();
            });
            SelectedMovieItemCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                InforMovieWindow w1 = new InforMovieWindow();
                LoadInforMovie(w1);
                w1.ShowDialog();
            });
            EditCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                    MessageBox.Show(SelectedItem.DisplayName);
            });
        }

        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }
        public void LoadInforMovie(InforMovieWindow w1)
        {
            DateTime temp = (DateTime)SelectedItem.ReleaseDate;

            w1.Name.Text = SelectedItem.DisplayName;
            w1.Genre.Text = SelectedItem.Genres.ToString();
            w1.Year.Text = temp.ToShortDateString();
            w1.Author.Text = SelectedItem.Director;
            w1.Country.Text = SelectedItem.Country;
            w1.Duration.Text = SelectedItem.RunningTime.ToString() + " phút";
            w1.Descripstion.Text = SelectedItem.Description;
        }

    }

}

