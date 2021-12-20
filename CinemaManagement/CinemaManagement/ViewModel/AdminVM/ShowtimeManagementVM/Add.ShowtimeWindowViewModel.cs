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

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }



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
                    IsSaving = false;
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();

                    p.Close();
                    ShadowMask.Visibility = Visibility.Collapsed;

                    await ReloadShowtimeList(-1);
                    GetShowingMovieByRoomInDate(SelectedRoomId);
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đầy đủ thông tin!", MessageType.Warning, MessageButtons.OK);
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
