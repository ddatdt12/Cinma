using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public partial class MainStaffViewModel : BaseViewModel
    {
        public ICommand LoadShowtimePageCM { get; set; }
        public ICommand LoadShowtimeDataCM { get; set; }

        public static bool IsShadow { get; set; }

        private bool _Shadow;
        public bool Shadow
        {
            get { return _Shadow; }
            set { _Shadow = value; OnPropertyChanged(); }
        }

        public async Task LoadShowtimeData()
        {
            LoadCurrentDate();   
            await LoadMainListBox(0);
        }

        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }

        public async Task LoadMainListBox(int func)
        {
            switch (func)
            {
                case 0:
                    {
                        ListMovie = new ObservableCollection<MovieDTO>(await Task.Run(() => MovieService.Ins.GetShowingMovieByDay(SelectedDate)));
                        break;
                    }
                case 1:
                    {
                        if (SelectedGenre != null)
                        {
                            await FilterMovieByGenre(SelectedGenre.Id);
                        }
                        break;
                    }
            }

        }

        public async Task FilterMovieByGenre(int _Id)
        {
            await Task.Run(() =>
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
            });
        }
    }
}
