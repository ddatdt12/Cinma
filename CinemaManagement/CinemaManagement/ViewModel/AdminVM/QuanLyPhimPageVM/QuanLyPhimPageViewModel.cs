using CinemaManagement.Views.Admin.QuanLyPhimPage;
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

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{
    public class QuanLyPhimPageViewModel : BaseViewModel
    {

        private DateTime _getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _getCurrentDate; }
            set { _getCurrentDate = value; }
        }

        private string _setCurrentDate;
        public string SetCurrentDate
        {
            get { return _setCurrentDate; }
            set { _setCurrentDate = value; }
        }

        public ICommand Open_AddMovieWindowCM { get; set; }

        public QuanLyPhimPageViewModel()
        {

            LoadCurrentDate();


            Open_AddMovieWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new AddMovieWindow();
                w1.ShowDialog();
            });

        }

        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }

    }
}

