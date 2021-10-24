using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementViewModel : BaseViewModel
    {
        private string _movieName;
        public string movieName
        {
            get { return _movieName; }
            set { _movieName = value; OnPropertyChanged(); }
        }

        private List<GenreDTO> _movieGenre;
        public List<GenreDTO> movieGenre
        {
            get { return _movieGenre; }
            set { _movieGenre = value; OnPropertyChanged(); }
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

        private DateTime _movieYear;
        public DateTime movieYear
        {
            get { return _movieYear; }
            set { _movieYear = value; OnPropertyChanged(); }
        }



        private MovieDTO _updatedMovie;
        public MovieDTO updateMovie
        {
            get { return _updatedMovie; }
            set
            {
                _updatedMovie = value;
                movieName = value.DisplayName;
                movieGenre = (List<GenreDTO>)value.Genres;
                movieDirector = value.Director;
                movieCountry = value.Country;
                movieDuration = value.RunningTime.ToString();
                movieYear = (DateTime)value.ReleaseDate;
                movieDes = value.Description;
            }
        }


        public ICommand UploadImageCM { get; set; }
        public ICommand SaveMovieCM { get; set; }
        public ICommand Add_SaveMovieCM { get; set; }
        public ICommand EditWindowLoadedCommand { get; set; }

        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;


        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
        }

        private List<string> _ListCountrySource;
        public List<string> ListCountrySource
        {
            get { return _ListCountrySource; }
            set { _ListCountrySource = value; OnPropertyChanged(); }
        }

        private List<int> _MinuteSource;
        public List<int> MinuteSource
        {
            get { return _MinuteSource; }
            set { _MinuteSource = value; OnPropertyChanged(); }
        }

        private List<GenreDTO> _GenreList;
        public List<GenreDTO> GenreList
        {
            get { return _GenreList; }
            set { _GenreList = value; OnPropertyChanged(); }
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



        public ICommand LoadAddMovieCM { get; set; }
        public ICommand LoadInforMovieCM { get; set; }
        public ICommand LoadEditMovieCM { get; set; }
        public ICommand LoadDeleteMovieCM { get; set; }

        public MovieManagementViewModel()
        {
            MovieList = new List<MovieDTO>(MovieService.Ins.GetAllMovie());
            ListCountrySource = new List<string>();
            MinuteSource = new List<int>();
            GenreList = new List<GenreDTO>(GenreService.Ins.GetAllGenre());

            InsertMinuteToComboBox();
            InsertCountryToComboBox();


            LoadAddMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new AddMovieWindow();
                w1.ShowDialog();
            });
            LoadInforMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                InforMovieWindow w1 = new InforMovieWindow();
                LoadInforMovie(w1);
                w1.ShowDialog();
            });
            LoadEditMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {

                    EditMovie w1 = new EditMovie();
                    LoadEditMovie(w1);
                    w1.ShowDialog();
                }
            });
            LoadDeleteMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
              {
                  System.Windows.MessageBox.Show("DELETED!");
              });

            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png|" + "All |*.*";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    AddLoadImage(openfile.FileName);
                    filepath = openfile.FileName;
                    extension = openfile.SafeFileName.Split('.')[1];
                    imgName = Helper.CreateImageName(movieName);
                    imgfullname = Helper.CreateImageFullName(imgName, extension);
                }
                else
                {
                    openfile.Dispose();
                }
            });

            //SaveMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            //{
            //    if (movieName != null && movieCountry != null && movieDirector != null && movieDes != null && imgName != null && movieGenre != null && movieYear != null && movieDuration != null)
            //    {
            //        MovieDTO movie = new MovieDTO
            //        {
            //            DisplayName = movieName,
            //            Country = movieCountry,
            //            Director = movieDirector,
            //            Description = movieDes,
            //            Image = imgfullname,
            //            Genres = movieGenre,
            //            ReleaseDate = movieYear.Date,
            //            RunningTime = int.Parse(movieDuration),
            //        };

            //        (bool successAddMovie, string messageFromAddMovie) = MovieService.Ins.AddMovie(movie);

            //        if (successAddMovie)
            //        {
            //            System.Windows.MessageBox.Show(messageFromAddMovie);
            //            SaveImgToApp();
            //            p.Close();

            //        }
            //        else
            //        {
            //            System.Windows.MessageBox.Show(messageFromAddMovie);
            //        }
            //    }
            //});

            Add_SaveMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (movieName != null && movieCountry != null && movieDirector != null && movieDes != null && imgName != null && movieGenre != null && movieYear != null && movieDuration != null)
                {
                    MovieDTO movie = new MovieDTO
                    {
                        DisplayName = movieName,
                        Country = movieCountry,
                        Director = movieDirector,
                        Description = movieDes,
                        Image = imgfullname,
                        Genres = movieGenre,
                        ReleaseDate = movieYear.Date,
                        RunningTime = int.Parse(movieDuration),
                    };

                    (bool successAddMovie, string messageFromAddMovie) = MovieService.Ins.AddMovie(movie);

                    if (successAddMovie)
                    {
                        System.Windows.MessageBox.Show(messageFromAddMovie);
                        SaveImgToApp();
                        p.Close();

                    }
                    else
                    {
                        System.Windows.MessageBox.Show(messageFromAddMovie);
                    }
                }
            });


        }

        public void LoadInforMovie(InforMovieWindow w1)
        {
            DateTime temp = (DateTime)SelectedItem.ReleaseDate;
            w1.Name.Text = SelectedItem.DisplayName;
            w1.Genre.Text = SelectedItem.Genres.ToString();
            w1.Year.Text = temp.ToShortDateString();
            w1.Author.Text = SelectedItem.Director;
            w1.Country.Text = SelectedItem.Country;
            w1.Duration.Text = SelectedItem.RunningTime.ToString();
            w1.Descripstion.Text = SelectedItem.Description;

            if (SelectedItem.Image != null)
            {
                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(Helper.GetMovieImgPath(SelectedItem.Image));
                _image.EndInit();

                w1.imageframe.Source = _image;
            }
        }
        public void LoadEditMovie(EditMovie w1)
        {
            w1._ID.Text = SelectedItem.Id.ToString();

            DateTime temp = (DateTime)SelectedItem.ReleaseDate;
            w1.Name.Text = SelectedItem.DisplayName;
            w1.Genre.Text = SelectedItem.Genres.ToString();
            w1.Year.Text = temp.ToShortDateString();
            w1.Author.Text = SelectedItem.Director;
            w1.Country.Text = SelectedItem.Country;
            w1.Duration.Text = SelectedItem.RunningTime.ToString();
            w1.Descripstion.Text = SelectedItem.Description;

            if (SelectedItem.Image != null)
            {
                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(Helper.GetMovieImgPath(SelectedItem.Image));
                _image.EndInit();

                w1.imgframe.Source = _image;
            }
        }
        public void AddLoadImage(string filePath)
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
                MovieDTO newMovie = new MovieDTO
                {
                    Image = imgfullname
                };
                appPath = Helper.GetMovieImgPath(imgfullname);

                File.Copy(filepath, $"{appPath}");

            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }
        public void InsertCountryToComboBox()
        {
            FileStream file = new FileStream(Helper.GetAdminPath("CountrySource.txt"), FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    ListCountrySource.Add(reader.ReadLine());
                }
            }
        }
        public void InsertMinuteToComboBox()
        {
            FileStream file = new FileStream(Helper.GetAdminPath("MinuteSource.txt"), FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    MinuteSource.Add(int.Parse(reader.ReadLine()));
                }
            }
        }

    }

}

