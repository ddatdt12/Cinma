using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Cache;
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
        bool IsImageChanged = false;
        bool IsAddingMovie = false;
        public static System.Windows.Controls.Grid MaskName { get; set; }

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
        public ICommand MaskNameCM { get; set; }

        public MovieManagementViewModel()
        {
            ListCountrySource = new List<string>();
            IsImageChanged = false;

            try
            {
                GenreList = GenreService.Ins.GetAllGenre();
                LoadMovieListView(Operation.READ);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống " + e.Message, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }

            InsertCountryToComboBox();

            StoreMainListViewNameCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                MainListView = p;
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });

            LoadAddMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                Window w1 = new AddMovieWindow();
                IsAddingMovie = true;
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();

            });
            LoadInforMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem == null) return;
                RenewWindowData();
                InforMovieWindow w1 = new InforMovieWindow();
                LoadInforMovie(w1);
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            LoadEditMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                oldMovieName = movieName;
                EditMovie w1 = new EditMovie();
                LoadEditMovie(w1);
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            LoadDeleteMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
              {
                  string message = "Bạn có chắc muốn xoá phim này không? Dữ liệu không thể phục hồi sau khi xoá!";
                  MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", message, MessageType.Warning, MessageButtons.YesNo);
                  result.ShowDialog();
                  switch (result.DialogResult)
                  {
                      case true:
                          {
                              (bool successDelMovie, string messageFromDelMovie) = MovieService.Ins.DeleteMovie(SelectedItem.Id);

                              if (successDelMovie)
                              {
                                  File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                                  LoadMovieListView(Operation.DELETE);
                                  SelectedItem = null;
                                  MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelMovie, MessageType.Success, MessageButtons.OK);
                                  mb.ShowDialog();
                                  break;
                              }
                              else
                              {
                                  MessageBoxCustom mb = new MessageBoxCustom("", messageFromDelMovie, MessageType.Error, MessageButtons.OK);
                                  mb.ShowDialog();
                                  break;
                              }
                          }
                      case false:
                          break;
                  }
              });
            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
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
                if (IsAddingMovie)
                {
                    appPath = Helper.GetMovieImgPath(imgfullname);
                    File.Copy(filepath, appPath, true);
                    return;
                }
                if (imgfullname != SelectedItem.Image)
                {
                    appPath = Helper.GetMovieImgPath(imgfullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetMovieImgPath(SelectedItem.Image)))
                        File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                }
                else
                {
                    string temp_name = imgfullname.Split('.')[0] + "temp";
                    string temp_ex = imgfullname.Split('.')[1];
                    string temp_fullname = Helper.CreateImageFullName(temp_name, temp_ex);

                    appPath = Helper.GetMovieImgPath(temp_fullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetMovieImgPath(imgfullname)))
                        File.Delete(Helper.GetMovieImgPath(imgfullname));
                    appPath = Helper.GetMovieImgPath(imgfullname);
                    filepath = Helper.GetMovieImgPath(temp_fullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetMovieImgPath(temp_fullname)))
                        File.Delete(Helper.GetMovieImgPath(temp_fullname));

                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }

        //Operation is enum have 4 values { READ, UPDATE, CREATE, DELETE }
        public void LoadMovieListView(Operation oper = Operation.READ, MovieDTO m = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    MovieList.Add(m);
                    break;
                case Operation.READ:
                    MovieList = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
                    break;
                case Operation.UPDATE:
                    var movieFound = MovieList.FirstOrDefault(x => x.Id == m.Id);
                    MovieList[MovieList.IndexOf(movieFound)] = m;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < MovieList.Count; i++)
                    {
                        if (MovieList[i].Id == SelectedItem?.Id)
                        {
                            MovieList.Remove(MovieList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
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
            ListCountrySource.Add("Ấn Độ");
            ListCountrySource.Add("Bồ Đào Nha");
            ListCountrySource.Add("Brazil");
            ListCountrySource.Add("Đài Loan");
            ListCountrySource.Add("Đức");
            ListCountrySource.Add("Hàn Quốc");
            ListCountrySource.Add("Hoa Kỳ");
            ListCountrySource.Add("Nga");
            ListCountrySource.Add("Nhật Bản");
            ListCountrySource.Add("Pháp");
            ListCountrySource.Add("Thái Lan");
            ListCountrySource.Add("Trung Quốc");
            ListCountrySource.Add("Việt Nam");

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
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(movieName) && movieCountry != null
                && !string.IsNullOrEmpty(movieDirector) && !string.IsNullOrEmpty(movieDes)
                 && movieGenre != null && !string.IsNullOrEmpty(movieYear)
                && !string.IsNullOrEmpty(movieDuration);
        }
    }

}

