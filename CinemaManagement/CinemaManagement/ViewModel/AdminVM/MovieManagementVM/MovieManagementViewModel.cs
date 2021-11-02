using CinemaManagement.DTOs;
using CinemaManagement.Models;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Cache;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        private string _movieYear;
        public string movieYear
        {
            get { return _movieYear; }
            set { _movieYear = value; OnPropertyChanged(); }
        }

        #endregion


        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;
        string oldMovieName;
        bool IsImageChanged;
        bool IsAddingMovie = false;

        System.Windows.Controls.ListView MainListView;


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



        public ICommand UploadImageCM { get; set; }
        public ICommand SaveMovieCM { get; set; }

        public ICommand LoadDeleteMovieCM { get; set; }
        public ICommand StoreMainListViewNameCM { get; set; }
        public ICommand UpdateMovieCM { get; set; }

        public MovieManagementViewModel()
        {
            MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
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
                RenewWindowData();
                Window w1 = new AddMovieWindow();
                IsAddingMovie = true;
                w1.ShowDialog();

            });
            LoadInforMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem == null) return;
                RenewWindowData();
                InforMovieWindow w1 = new InforMovieWindow();
                LoadInforMovie(w1);
                w1.ShowDialog();
            });
            LoadEditMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                oldMovieName = movieName;
                EditMovie w1 = new EditMovie();
                LoadEditMovie(w1);
                w1.ShowDialog();

            });
            LoadDeleteMovieCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
              {
                  MessageBoxResult result = System.Windows.MessageBox.Show("Bạn có chắc muốn xoá phim này không ? Dữ liệu không thể phục hồi sau khi xoá!", "Xác nhận xoá", MessageBoxButton.YesNo);
                  switch (result)
                  {
                      case MessageBoxResult.Yes:
                          {
                              (bool successDelMovie, string messageFromDelMovie) = MovieService.Ins.DeleteMovie(SelectedItem.Id);

                              if (successDelMovie)
                              {

                                  File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                                  System.Windows.MessageBox.Show(messageFromDelMovie);
                                  ReloadMovieListView(SelectedItem.DisplayName);
                                  SelectedItem = null;
                                  break;
                              }
                              else
                              {
                                  System.Windows.MessageBox.Show(messageFromDelMovie);
                                  break;
                              }
                          }

                      case MessageBoxResult.No:
                          break;
                  }
              });

            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png|" + "All |*.*";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    extension = openfile.SafeFileName.Split('.')[1];
                    LoadImage();
                    return;
                }
                IsImageChanged = false;

            });
            UpdateMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                UpdateMovieFunc(p);

            });
            SaveMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SaveMovieFunc(p);
            });
        }

        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        public void SaveImgToApp()
        {
            try
            {
                appPath = Helper.GetMovieImgPath(imgfullname);

                File.Copy(filepath, $"{appPath}");
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }
        public void ReloadMovieListView(string name = "")
        {
            if (name == "add")
            {
                MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
                return;
            }

            if (name != "")
            {
                for (int i = 0; i < MovieList.Count; i++)
                {
                    if (MovieList[i].DisplayName == name)
                        MovieList.Remove(MovieList[i]);
                }
            }
            if (name == "")
                MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
            MainListView.Items.Refresh();
        }
        public void RenewWindowData()
        {
            movieName = null;
            movieGenre = null;
            movieDirector = null;
            movieCountry = null;
            movieDuration = null;
            movieDes = null;
            ImageSource = null;
            movieYear = null;
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
        public void MovieImageChanged()
        {
            if (!IsAddingMovie)
            {
                if (movieName != null && IsImageChanged == true)
                {
                    extension = SelectedItem.Image.Split('.')[1];
                    imgName = Helper.CreateImageName(movieName);
                    imgfullname = Helper.CreateImageFullName(imgName, extension);
                }
            }
        }
    }

}

