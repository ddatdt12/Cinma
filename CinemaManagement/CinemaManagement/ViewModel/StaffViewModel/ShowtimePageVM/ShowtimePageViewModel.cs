using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public partial class MainStaffViewModel : BaseViewModel
    {
        public ICommand LoadShowtimePageCM { get; set; }
        public ICommand LoadShowtimeDataCM { get; set; }

        public void LoadShowtimeData()
        {
            LoadCurrentDate();
            LoadMainListBox(0);
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
