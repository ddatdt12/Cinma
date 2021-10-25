using CinemaManagement.DTOs;
using CinemaManagement.Models;
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

        #region   bien de luu du lieu up cho database

        private string _movieID;
        public string movieID
        {
            get { return _movieID; }
            set { _movieID = value; OnPropertyChanged(); }
        }

        private string _movieName;
        public string movieName
        {
            get { return _movieName; }
            set { _movieName = value; OnPropertyChanged(); }
        }

        private GenreDTO _movieGenre;
        public GenreDTO movieGenre
        {
            get => _movieGenre;
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
                movieGenre = (GenreDTO)value.Genres;
                movieDirector = value.Director;
                movieCountry = value.Country;
                movieDuration = value.RunningTime.ToString();
                movieYear = (DateTime)value.ReleaseDate;
                movieDes = value.Description;
            }
        }

        #endregion


        public ICommand UploadImageCM { get; set; }
        public ICommand SaveMovieCM { get; set; }
        public ICommand Add_SaveMovieCM { get; set; }
        public ICommand EditWindowLoadedCommand { get; set; }

        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;
        System.Windows.Controls.ListView MainListView;


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
            get => _GenreList;
            set
            {
                _GenreList = value;
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



        public ICommand LoadAddMovieCM { get; set; }
        public ICommand LoadInforMovieCM { get; set; }
        public ICommand LoadEditMovieCM { get; set; }
        public ICommand LoadDeleteMovieCM { get; set; }
        public ICommand StoreMainListViewNameCM { get; set; }

        public MovieManagementViewModel()
        {
            MovieList = new List<MovieDTO>(MovieService.Ins.GetAllMovie());
            ListCountrySource = new List<string>();
            MinuteSource = new List<int>();
            GenreList = GenreService.Ins.GetAllGenre();

            InsertMinuteToComboBox();
            InsertCountryToComboBox();

            StoreMainListViewNameCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                MainListView = p;
            });
            LoadAddMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new AddMovieWindow();
                RenewAddWindowData();
                w1.ShowDialog();
            });
            LoadInforMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                RenewAddWindowData();
                InforMovieWindow w1 = new InforMovieWindow();
                LoadInforMovie(w1);
                w1.ShowDialog();
            });
            LoadEditMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                RenewAddWindowData();
                EditMovie w1 = new EditMovie();
                LoadEditMovie(w1);
                w1.ShowDialog();

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
                    LoadImage(openfile.FileName);
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

            SaveMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (movieID == null && movieName != null && movieCountry != null && movieDirector != null && movieDes != null && imgName != null && movieGenre != null && movieYear != null && movieDuration != null)
                {
                    List<GenreDTO> temp = new List<GenreDTO>();
                    temp.Add(movieGenre);


                    MovieDTO movie = new MovieDTO
                    {

                        DisplayName = movieName,
                        Country = movieCountry,
                        Director = movieDirector,
                        Description = movieDes,
                        Image = imgfullname,
                        Genres = temp,
                        ReleaseDate = movieYear,
                        RunningTime = int.Parse(movieDuration),
                    };

                    (bool successAddMovie, string messageFromAddMovie) = MovieService.Ins.AddMovie(movie);

                    if (successAddMovie)
                    {
                        System.Windows.MessageBox.Show(messageFromAddMovie);
                        SaveImgToApp();
                        p.Close();
                        ReloadMovieListView();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(messageFromAddMovie);
                    }
                }
                else if (movieID != null)
                {
                    System.Windows.MessageBox.Show("sau khi edit luu thanh cong");
                }
            });

        }

        public void LoadInforMovie(InforMovieWindow w1)
        {
            List<GenreDTO> tempgenre = new List<GenreDTO>(SelectedItem.Genres);

            DateTime temp = (DateTime)SelectedItem.ReleaseDate;
            movieName = SelectedItem.DisplayName;
            w1.Genre.Text = tempgenre[0].DisplayName;
            movieDirector = SelectedItem.Director;
            movieCountry = SelectedItem.Country;
            movieDuration = SelectedItem.RunningTime.ToString();
            movieDes = SelectedItem.Description;
            movieYear = (DateTime)SelectedItem.ReleaseDate;
            w1.Year.Text = SelectedItem.ReleaseDate.ToString();

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
            List<GenreDTO> tempgenre = new List<GenreDTO>(SelectedItem.Genres);

            movieID = SelectedItem.Id.ToString();
            movieName = SelectedItem.DisplayName;
            movieGenre = tempgenre[0];
            movieYear = (DateTime)SelectedItem.ReleaseDate;
            movieDirector = SelectedItem.Director;
            movieCountry = SelectedItem.Country;
            movieDuration = SelectedItem.RunningTime.ToString();
            movieDes = SelectedItem.Description;
            w1._Genre.Text = tempgenre[0].DisplayName;

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
        public void ReloadMovieListView()
        {
            MainListView.ItemsSource = new List<MovieDTO>(MovieService.Ins.GetAllMovie());
        }
        public void RenewAddWindowData()
        {
            movieName = null;
            movieGenre = null;
            movieDirector = null;
            movieCountry = null;
            movieDuration = null;
            movieDes = null;
            ImageSource = null;
            movieYear = DateTime.Today;
        }
    }

}

