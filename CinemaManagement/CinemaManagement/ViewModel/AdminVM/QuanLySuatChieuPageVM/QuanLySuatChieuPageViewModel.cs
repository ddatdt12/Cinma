using CinemaManagement.Views.Admin.QuanLySuatChieuPage;
using System;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLySuatChieuPageVM
{
    public class QuanLySuatChieuPageViewModel : BaseViewModel
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




        public ICommand Open_AddSuatChieuWindowCM { get; set; }




        public QuanLySuatChieuPageViewModel()
        {
            LoadCurrentDate();
            Open_AddSuatChieuWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddSuatChieuWindow temp = new AddSuatChieuWindow();
                temp.ShowDialog();
            });
        }

        public void LoadCurrentDate()
        {
            GetCurrentDate = DateTime.Now.Date;
            SetCurrentDate = GetCurrentDate.ToShortDateString();
        }

    }
}
