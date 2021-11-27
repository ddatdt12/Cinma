using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM;
using CinemaManagement.Views.LoginWindow;
using CinemaManagement.Views.Staff;
using CinemaManagement.Views.Staff.MovieScheduleWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel
{
    public partial class MainStaffViewModel : BaseViewModel
    {
        private ObservableCollection<MovieDTO> _ListMovie;
        public ObservableCollection<MovieDTO> ListMovie
        {
            get { return _ListMovie; }
            set { _ListMovie = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MovieDTO> _ListMovie1;
        public ObservableCollection<MovieDTO> ListMovie1
        {
            get { return _ListMovie1; }
            set { _ListMovie1 = value; }
        }

        private ImageSource _ImgSource;
        public ImageSource ImgSource
        {
            get { return _ImgSource; }
            set { _ImgSource = value; }
        }

        private MovieDTO _SelectedItem;
        public MovieDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged(); LoadMainListBox(0); }
        }

        private GenreDTO _SelectedGenre;
        public GenreDTO SelectedGenre
        {
            get { return _SelectedGenre; }
            set { _SelectedGenre = value; OnPropertyChanged(); LoadMainListBox(1); }
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
            set { _setCurrentDate = value; OnPropertyChanged(); }
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
        #region commands
        public ICommand CloseMainStaffWindowCM { get; set; }
        public ICommand MinimizeMainStaffWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand CategoryFilmLabel { get; set; }
        public ICommand LoadMainStaffCM { get; set; }
        public ICommand LoadMovieScheduleWindow { get; set; }
        public ICommand SignoutCM { get; set; }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }


        #endregion
        public MainStaffViewModel()
        {
            LoadCurrentDate();
            ListMovie1 = new ObservableCollection<MovieDTO>(MovieService.Ins.GetAllMovie());
            GenreList = GenreService.Ins.GetAllGenre();
            CloseMainStaffWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
                {
                    FrameworkElement window = Window.GetWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.Close();
                    }
                });
            MinimizeMainStaffWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
                {
                    FrameworkElement window = Window.GetWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                });
            MouseMoveWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
                {
                    FrameworkElement window = Window.GetWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.DragMove();
                    }
                });
            CategoryFilmLabel = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
                {
                    if (p != null)
                    {
                        if (p.Visibility == Visibility.Collapsed)
                        {
                            p.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            p.Visibility = Visibility.Collapsed;
                        }
                    }
                });


            LoadMovieScheduleWindow = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                MovieScheduleWindow w;
                if (SelectedItem != null)
                {
                    w = new MovieScheduleWindow();
                    w._ShowTimeList.ItemsSource = SelectedItem.Showtimes;
                    w.imgframe.Source = Helper.GetMovieImageSource(SelectedItem.Image);
                    w._ShowDate.Text = SelectedDate.ToString("dd-MM-yyyy");
                    w.txtframe.Text = SelectedItem.DisplayName;
                    MovieScheduleWindowViewModel.tempFilebinding = SelectedItem;
                    w.ShowDialog();
                }
            });
            LoadMainStaffCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadMainListBox(0);
                SetCurrentDate = GetCurrentDate.ToString();
            });
            SignoutCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                LoginWindow w1 = new LoginWindow();
                w1.ShowDialog();
                p.Close();
            });
        }
        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }
        public void LoadMainListBox(int func)
        {
            switch (func)
            {
                case 0:
                    {
                        ListMovie = new ObservableCollection<MovieDTO>(MovieService.Ins.GetShowingMovieByDay(SelectedDate));
                        break;
                    }
                case 1:
                    {
                        FilterMovieByGenre(SelectedGenre.Id);
                        break;
                    }
            }

        }

        public void FilterMovieByGenre(int _Id)
        {
            ObservableCollection<MovieDTO> byGenre = new ObservableCollection<MovieDTO>();

            foreach (var item in ListMovie1)
            {
                if (item.Genres[0].Id == _Id)
                {
                    byGenre.Add(item);
                }
            }
            ListMovie = new ObservableCollection<MovieDTO>(byGenre);
        }
    }
}