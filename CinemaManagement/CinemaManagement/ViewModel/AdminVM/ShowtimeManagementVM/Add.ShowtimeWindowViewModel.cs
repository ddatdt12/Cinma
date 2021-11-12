using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Windows;
using System.Windows.Input;


namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {

        private DateTime _EndTime;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; OnPropertyChanged(); }
        }


        public ICommand LoadAddShowtimeCM { get; set; }
        public ICommand SaveCM { get; set; }


        public void SaveShowtimeFunc(Window p)
        {
            if (IsValidData())
            {

                ShowtimeDTO temp = new ShowtimeDTO
                {
                    MovieId = movieSelected.Id,
                    RoomId = ShowtimeRoom.Id,
                    ShowDate = showtimeDate,
                    StartTime = Showtime.TimeOfDay,
                    TicketPrice = moviePrice,
                };

                (bool IsSuccess, string message, ShowtimeDTO newShowtime) = ShowtimeService.Ins.AddShowtime(temp);


                if (IsSuccess)
                {
                    MessageBox.Show(message);
                    ReloadShowtimeList(SelectedRoomId);
                    p.Close();
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            else
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
        }
        public void CalculateRunningTime()
        {
            
            if (movieSelected != null)
            {
                EndTime = Showtime.AddMinutes(movieSelected.RunningTime);
            }
        }
        public void RenewData()
        {
            movieSelected = null;
            showtimeDate = GetCurrentDate;
            ShowtimeRoom = null;
            Showtime = new DateTime();
            EndTime = new DateTime();
            moviePrice = 45000;
        }
    }
}
