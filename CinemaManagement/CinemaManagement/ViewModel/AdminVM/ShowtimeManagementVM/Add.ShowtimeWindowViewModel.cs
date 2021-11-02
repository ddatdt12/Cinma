using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {

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
                };

                (bool IsSuccess, string message) = ShowtimeService.Ins.AddShowtime(temp);


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
    }
}
