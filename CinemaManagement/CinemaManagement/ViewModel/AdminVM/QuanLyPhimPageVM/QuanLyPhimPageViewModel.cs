using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System.Windows.Media.Imaging;
using System.Net.Cache;
using System.IO;

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
        
        private string _TextFilterChanged;
        public string TextFilterChanged
        {
            get { return _TextFilterChanged; }
            set { _TextFilterChanged = value; OnPropertyChanged(); }
        }

        public ImageSource ImageSource { get; set; }
        public void LoadImage()
        {
            ImageSource = new BitmapImage(new Uri(@"/CinemaManagement;component/Resources/LayoutSignin/bo-gia.jpg", UriKind.Relative));
        }


        public ICommand Open_AddMovieWindowCM { get; set; }
        public ICommand SelectedMovieItemCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCM { get; set; }

        public QuanLyPhimPageViewModel()
        {
            LoadImage();
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
                {
                    EditMovie w1 = new EditMovie();
                    LoadEditMovie(w1);
                    w1.ShowDialog();
                }
            });
            DeleteCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
              {
                  MessageBox.Show("DELETED!");
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
        public void LoadEditMovie(EditMovie w1)
        {

            DateTime temp = (DateTime)SelectedItem.ReleaseDate;

            w1._ID.Text= SelectedItem.Id.ToString();
            w1._Displayname.Text = SelectedItem.DisplayName;
            w1._Genre.Text = SelectedItem.Genres.ToString();
            w1._Year.Text = temp.Year.ToString();
            w1._Author.Text = SelectedItem.Director;
            w1._Country.Text = SelectedItem.Country;
            w1._Duration.Text = SelectedItem.RunningTime.ToString();
            w1._Description.Text = SelectedItem.Description;


            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache; 
            _image.UriSource = new Uri(Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images\Movies", "add-user.png" /*SelectedItem.Image*/));
            _image.EndInit();
        
            w1.imgframe.Source = _image;

        }
    }

}

