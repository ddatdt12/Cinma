using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
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
            if (ListMovie != null)
                ListMovie.Clear();
            switch (func)
            {
                case 0:
                    {
                        try
                        {
                            IsLoading = true;
                            ListMovie = new ObservableCollection<MovieDTO>(await Task.Run(() => MovieService.Ins.GetShowingMovieByDay(SelectedDate)));
                            IsLoading = false;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                        }
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
