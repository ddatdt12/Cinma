using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM;
using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.Views;
using CinemaManagement.Views.LoginWindow;
using CinemaManagement.Views.Staff.TroubleWindow;
using CinemaManagement.Views.Staff.MovieScheduleWindow;
using CinemaManagement.Views.Staff.OrderFoodWindow;
using CinemaManagement.Views.Staff.ShowtimePage;
using System;
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
            set { _SelectedDate = value; OnPropertyChanged(); }
        }

        private GenreDTO _SelectedGenre;
        public GenreDTO SelectedGenre
        {
            get { return _SelectedGenre; }
            set { _SelectedGenre = value; OnPropertyChanged(); }
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

        private ObservableCollection<GenreDTO> _GenreList;
        public ObservableCollection<GenreDTO> GenreList
        {
            get => _GenreList;
            set
            {
                _GenreList = value; OnPropertyChanged();
            }
        }

        public static Grid MaskName { get; set; }
        public static StaffDTO CurrentStaff { get; set; }

        #region commands
        public ICommand CloseMainStaffWindowCM { get; set; }
        public ICommand MinimizeMainStaffWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand LoadMovieScheduleWindow { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand SelectedGenreCM { get; set; }
        public ICommand SelectedDateCM { get; set; }
        public ICommand LoadErrorPageCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand ChangeRoleCM { get; set; }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged(); }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }


        #endregion
        public MainStaffViewModel()
        {
            SelectedGenreCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await LoadMainListBox(1);
                IsLoading = false;
            });
            SelectedDateCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await LoadMainListBox(0);
                IsLoading = false;
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 if (CurrentStaff.Role == "Quản lý")
                     IsAdmin = true;
                 else
                     IsAdmin = false;

                 LoadCurrentDate();
                 SelectedDate = GetCurrentDate;
                 ListMovie1 = new ObservableCollection<MovieDTO>(await MovieService.Ins.GetShowingMovieByDay(SelectedDate));
                 GenreList = new ObservableCollection<GenreDTO>(GenreService.Ins.GetAllGenre());
             });
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
            LoadMovieScheduleWindow = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {

                MovieScheduleWindow w;
                OrderFoodPageViewModel.checkOnlyFoodOfPage = false;

                if (SelectedItem != null)
                {
                    try
                    {
                        MovieScheduleWindowViewModel.tempFilebinding = SelectedItem;
                        w = new MovieScheduleWindow();

                        if (w != null)
                        {
                            MaskName.Visibility = Visibility.Visible;
                            if (SelectedItem != null)
                            {
                                w._ShowTimeList.ItemsSource = SelectedItem.Showtimes;
                                w.imgframe.Source = await CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image);
                                w._ShowDate.Text = SelectedDate.ToString("dd-MM-yyyy");
                                w.txtframe.Text = SelectedItem?.DisplayName ?? "";
                                w.ShowDialog();
                            }
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                }
            });
            LoadShowtimePageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                OrderFoodPageViewModel.checkOnlyFoodOfPage = false;
                p.Content = new ShowtimePage();

            });
            LoadFoodPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                if (OrderFoodPageViewModel.ListOrder != null)
                {
                    OrderFoodPageViewModel.ListOrder.Clear();
                }
                OrderFoodPageViewModel.checkOnlyFoodOfPage = true;
                p.Content = new FoodPage();

            });
            LoadShowtimeDataCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
             {
                 p.SelectedIndex = -1;
                 await LoadShowtimeData();
             });
            LoadErrorPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
             {
                 p.Content = new TroublePage();
             });
            SignoutCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                LoginWindow w1 = new LoginWindow();
                w1.ShowDialog();
                p.Close();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            ChangeRoleCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                MainAdminWindow w1 = new MainAdminWindow();
                MainAdminViewModel.currentStaff = CurrentStaff;
                w1.CurrentUserName.Content = CurrentStaff.Name;
                w1.Show();
                p.Close();
            });
        }
    }
}