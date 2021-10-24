using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.QuanLyPhimPage;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{
    public class InforMovieWindowViewModel : BaseViewModel
    {
        private string _DisplayName;

        public string DisplayName
        {
            get { return _DisplayName; }
            set {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetNameCM { get; set; }

        public InforMovieWindowViewModel()
        {
            GetNameCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                Console.WriteLine(DisplayName);

            });
        }

    }
}
