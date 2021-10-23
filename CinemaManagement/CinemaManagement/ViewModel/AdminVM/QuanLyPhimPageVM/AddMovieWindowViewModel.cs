using CinemaManagement.DTOs;
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

        private string _movieGenre;
        public string movieGenre
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





        private List<string> _ListCountrySource = new List<string>();
        public List<string> ListCountrySource
        {
            get { return _ListCountrySource; }
            set { _ListCountrySource.Add(value.ToString()); OnPropertyChanged(); }
        }

        private List<string> _ListMovieGenreSource = new List<string>();
        public List<string> ListMovieGenreSource
        {
            get { return _ListMovieGenreSource; }
            set { _ListMovieGenreSource.Add(value.ToString()); OnPropertyChanged(); }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        public ICommand UploadImageCM { get; set; }
        public ICommand SaveMovieCM { get; set; }

        string filepath;
        string appPath;
        string img;
        public AddMovieWindowViewModel()
        {
            InsertCountryToComboBox();
            InsertMovieGerneToComboBox();

            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png|" + "All |*.*";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(openfile.FileName);

                    try
                    {
                        appPath = Helper.GetMovieImgPath();
                        string MovieName = movieName;
                        string imgName = Helper.CreateImageName(MovieName);
                        img = Helper.CreateImageFullName(imgName, openfile.SafeFileName.Split('.')[1]);
                        MovieDTO newMovie = new MovieDTO
                        {
                            Image = img
                        };

                        filepath = openfile.FileName;



                    }
                    catch (Exception exp)
                    {
                        System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
                    }
                }
                else
                {
                    openfile.Dispose();
                }

            });
            bool issave = false;
            SaveMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SaveImgToApp(filepath, appPath, img);
            });
        }


        public void InsertCountryToComboBox()
        {
            FileStream file = new FileStream(@"CountrySource.txt", FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    ListCountrySource.Add(reader.ReadLine());
                }
            }
        }
        public void InsertMovieGerneToComboBox()
        {
            FileStream file = new FileStream(@"MovieGenreSource.txt", FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    ListMovieGenreSource.Add(reader.ReadLine());
                }
            }
        }
        public void LoadImage(string filepath)
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
        public void SaveImgToApp(string filepath, string appPath, string img)
        {
            File.Copy(filepath, $"{appPath}{img}");
        }
    }
}
