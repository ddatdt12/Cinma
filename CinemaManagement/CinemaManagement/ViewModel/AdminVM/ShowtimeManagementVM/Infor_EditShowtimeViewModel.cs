using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.ShowtimeManagement;
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



        public ICommand LoadInfor_EditShowtime { get; set; }
        public ICommand CloseEditCM { get; set; }


        private ShowtimeDTO _selectedShowtime; //the showtime being selected
        public ShowtimeDTO SelectedShowtime
        {
            get { return _selectedShowtime; }
            set
            {
                _selectedShowtime = value;
                OnPropertyChanged();
                SeatFunc();
            }
        }


        public void Infor_EditFunc()
        {
            if (SelectedItem != null)
            {
                Infor_EditShowtimeWindow p = new Infor_EditShowtimeWindow();
                LoadDataEditWindow(p);
                p.ShowDialog();
            }
        }


        public void LoadDataEditWindow(Infor_EditShowtimeWindow p)
        {
            p._movieName.Text = SelectedItem.DisplayName;
            p._ShowtimeDate.Text = SelectedDate.ToString("dd-MM-yyyy");

            if (SelectedRoomId == -1)
                p._ShowtimeRoom.Text = "Phòng: Toàn bộ ";
            else
                p._ShowtimeRoom.Text = "Phòng: " + SelectedRoomId.ToString();

            p._Showtime.ItemsSource = SelectedItem.Showtimes;
        }

        public void SeatFunc()
        {
            if (SelectedShowtime != null)
                ListSeat = SelectedItem.DisplayName + "\n" + SelectedShowtime.StartTime.ToString();
        }
    }
}
