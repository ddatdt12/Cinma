using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.ShowtimeManagement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {
        private string _ListSeat;
        public string ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; OnPropertyChanged(); }
        }

        private Infor_EditShowtimeWindow _EditShowtimeWindow;
        public Infor_EditShowtimeWindow EditShowtimeWindow
        {
            get { return _EditShowtimeWindow; }
            set { _EditShowtimeWindow = value; }
        }

        private ObservableCollection<ShowtimeDTO> _ListShowtimeofMovie;
        public ObservableCollection<ShowtimeDTO> ListShowtimeofMovie
        {
            get { return _ListShowtimeofMovie; }
            set { _ListShowtimeofMovie = value; OnPropertyChanged(); }
        }




        public ICommand LoadInfor_EditShowtime { get; set; }
        public ICommand CloseEditCM { get; set; }
        public ICommand LoadSeatCM { get; set; }


        private ShowtimeDTO _selectedShowtime; //the showtime being selected
        public ShowtimeDTO SelectedShowtime
        {
            get { return _selectedShowtime; }
            set
            {
                _selectedShowtime = value;
                OnPropertyChanged();
            }
        }


        public void Infor_EditFunc()
        {
            if (SelectedItem != null)
            {
                Infor_EditShowtimeWindow p = new Infor_EditShowtimeWindow();
                LoadDataEditWindow(p);
                EditShowtimeWindow = p;
                oldSelectedItem = SelectedItem;
                p.ShowDialog();
            }
        }


        public void LoadDataEditWindow(Infor_EditShowtimeWindow p)
        {
            p._movieName.Text = SelectedItem.DisplayName;
            p._ShowtimeDate.Text = SelectedDate.ToString("dd-MM-yyyy");
            p._showtimePrice.Text = "";

            if (SelectedRoomId == -1)
                p._ShowtimeRoom.Text = "Phòng: Toàn bộ ";
            else
                p._ShowtimeRoom.Text = "Phòng: " + SelectedRoomId.ToString();

            ListShowtimeofMovie = new ObservableCollection<ShowtimeDTO>(SelectedItem.Showtimes);
            ListSeat = "";

            moviePrice = 0;
        }

    }
}
