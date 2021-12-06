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

        public async void LoadShowtimeData()
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
                        ListMovie = new ObservableCollection<MovieDTO>(await MovieService.Ins.GetShowingMovieByDay(SelectedDate));
                        break;
                    }
                case 1:
                    {
                        if (SelectedGenre != null)
                        {
                            FilterMovieByGenre(SelectedGenre.Id);
                        }
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
