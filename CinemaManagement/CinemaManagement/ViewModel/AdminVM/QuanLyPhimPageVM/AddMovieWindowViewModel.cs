using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Cache;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{

    public class AddMovieWindowViewModel : BaseViewModel
    {
        private string _movieName;
        public string movieName
        {
            get { return _movieName; }
            set { _movieName = value; OnPropertyChanged(); }
        }

        private List<string> _ListCountrySource;
        public List<string> ListCountrySource
        {
            get { return _ListCountrySource; }
            set { _ListCountrySource = value; OnPropertyChanged(); }
        }

        private string _movieDirector;
        public string movieDirector
        {
            get { return _movieDirector; }
            set { _movieDirector = value; OnPropertyChanged(); }
        }

        private string _movieCountry;
        public string movieCountry
        {
            get { return _movieCountry; }
            set { _movieCountry = value; OnPropertyChanged(); }
        }

        private string _movieDuration;
        public string movieDuration
        {
            get { return _movieDuration; }
            set { _movieDuration = value; OnPropertyChanged(); }
        }

        private string _movieYear;
        public string movieYear
        {
            get { return _movieYear; }
            set { _movieYear = value; OnPropertyChanged(); }
        }

        private string _movieDes;
        public string movieDes
        {
            get { return _movieDes; }
            set { _movieDes = value; OnPropertyChanged(); }
        }


        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }


        private GenreDTO _movieGenre;
        public GenreDTO movieGenre

        {
            get { return _movieGenre; }
            set { _movieGenre = value; OnPropertyChanged(); }
        }


      

        private List<GenreDTO> _GenreList = new List<GenreDTO>();
        public List<GenreDTO> GenreList
        {
            get { return _GenreList; }
            set { _GenreList = value ; OnPropertyChanged(); }
        }

 

        public ICommand UploadImageCM { get; set; }
        public ICommand SaveMovieCM { get; set; }

        string filepath;
        string appPath;
        string img;
        string splitname;
        
        public AddMovieWindowViewModel()
        {
            InsertCountryToComboBox();

            
            GenreList = GenreService.Ins.GetAllGenre();

            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png|" + "All |*.*";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(openfile.FileName);
                    filepath = openfile.FileName;
                    splitname = openfile.SafeFileName.Split('.')[1];
                }
                else
                {
                    openfile.Dispose();
                }
            });
            

            SaveMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                List<GenreDTO> genreDTOs = new List<GenreDTO>();
                genreDTOs.Add(movieGenre);
                MovieDTO movie = new MovieDTO
                {
                    DisplayName = movieName,
                    Country = movieCountry,
                    Director = movieDirector,
                    Description = movieDes,
                    Image = img,
                    Genres= genreDTOs,
                    ReleaseDate =DateTime.Now.Date,
                    RunningTime=190,
                };
                (bool successAddMovie, string messageFromAddMovie) = MovieService.Ins.AddMovie(movie);
                if (successAddMovie)
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                    SaveImgToApp();
                }

            }
        }


                else
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                }
            });
        }


        //public void InsertCountryToComboBox()
        //{
        //    FileStream file = new FileStream(@"CountrySource.txt", FileMode.Open, FileAccess.Read);
        //    using (var reader = new StreamReader(file, Encoding.UTF8))
        //    {
        //        while (reader.Peek() >= 0)
        //        {
        //            ListCountrySource.Add(reader.ReadLine());
        //        }
        //    }
        //}
        //public void InsertMovieGerneToComboBox()
        //{
        //    FileStream file = new FileStream(@"MovieGenreSource.txt", FileMode.Open, FileAccess.Read);
        //    using (var reader = new StreamReader(file, Encoding.UTF8))
        //    {
        //        while (reader.Peek() >= 0)
        //        {
        //            ListMovieGenreSource.Add(reader.ReadLine());
        //        }
        //    }
        //}
        public void LoadImage(string filePath)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        public void SaveImgToApp()
        {
            try
            {
                appPath = Helper.GetMovieImgPath();
                string MovieName = movieName;
                string imgName = Helper.CreateImageName(MovieName);
                img = Helper.CreateImageFullName(imgName, splitname);
                MovieDTO newMovie = new MovieDTO
                {
                    Image = img
                };

                File.Copy(filepath, $"{appPath}{img}");

            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }
    }
}
