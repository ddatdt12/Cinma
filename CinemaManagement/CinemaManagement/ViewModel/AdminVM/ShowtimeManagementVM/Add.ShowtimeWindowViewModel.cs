using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using System;
using System.Threading.Tasks;
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


        public async Task SaveShowtimeFunc(Window p)
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

                (bool IsSuccess, string message) = await ShowtimeService.Ins.AddShowtime(temp);

                if (IsSuccess)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    await ReloadShowtimeList(SelectedRoomId);
                    ShadowMask.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Vui lòng nhập đầy đủ thông tin!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
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
