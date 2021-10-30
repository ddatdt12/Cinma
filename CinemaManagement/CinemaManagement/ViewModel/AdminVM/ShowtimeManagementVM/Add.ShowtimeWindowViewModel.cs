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

        }
    }
}
