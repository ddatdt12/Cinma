using CinemaManagement.DTOs;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using CinemaManagement.Views.Staff.TicketWindow;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM
{
    public class MovieScheduleWindowViewModel : BaseViewModel
    {
        #region
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand VisibleSeat { get; set; }
        public ICommand CloseCM { get; set; }

        private List<ShowtimeDTO> _ListShowtime;
        public List<ShowtimeDTO> ListShowtime
        {
            get { return _ListShowtime; }
            set { _ListShowtime = value; OnPropertyChanged(); }
        }


        private ShowtimeDTO _selectedShowtime;

        public ShowtimeDTO SelectedShowtime
        {
            get { return _selectedShowtime; }
            set { _selectedShowtime = value; OnPropertyChanged(); GetShowtimeRoom(); }
        }

        private string _ShowTimeRoom;
        public string ShowTimeRoom
        {
            get { return _ShowTimeRoom; }
            set { _ShowTimeRoom = value; OnPropertyChanged(); }
        }

        public static MovieDTO tempFilebinding;
        #endregion
        public MovieScheduleWindowViewModel()
        {
            VisibleSeat = new RelayCommand<object>((p) => { return true; }, (p) =>
             {
                 if (ShowTimeRoom != null)
                 {
                     TicketWindowViewModel.CurrentShowtime = SelectedShowtime;
                     TicketWindowViewModel.tempFilmName = tempFilebinding;
                     TicketWindowViewModel.showTimeRoom = ShowTimeRoom;
                     TicketWindow w = new TicketWindow();
                     w.ShowDialog();
                 }
             });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MainStaffViewModel.MaskName.Visibility = Visibility.Collapsed;
                p.Close();
            });

        }
        public void GetShowtimeRoom()
        {
            ShowTimeRoom = "Phòng 0" + SelectedShowtime.RoomId.ToString();
        }
    }
}
