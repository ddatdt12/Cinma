using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyNhanVienPageVM
{
    public class NhanVienPageViewModel : BaseViewModel
    {
        private List<StaffDTO> _staffList;
        public List<StaffDTO> StaffList
        {
            get => _staffList;
            set
            {
                _staffList = value;
            }
        }

        public ICommand AddStaffCommand { get; set; }
        public ICommand EditStaffCommand { get; set; }
        public ICommand DeleteStaffCommand { get; set; }
        
        private StaffDTO _SelectedItem;
        public StaffDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }



        public NhanVienPageViewModel()
        {
            AddStaffCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ThemNhanVienWindow wd = new ThemNhanVienWindow(); wd.ShowDialog(); });
            EditStaffCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                SuaNhanVienWindow wd = new SuaNhanVienWindow();

                wd.FullName.Text = SelectedItem.Name;
                wd.Gender.Text = SelectedItem.Gender;
                wd.Date.Text = SelectedItem.BirthDate.ToString();
                wd.Phone.Text = SelectedItem.PhoneNumber.ToString();
                wd.Role.Text = SelectedItem.Role;
                wd.StartDate.Text = SelectedItem.StartingDate.ToString();
                
                wd.ShowDialog(); });
            DeleteStaffCommand = new RelayCommand<object>((p) => { return true; }, (p) => { XoaNhanVienWindow wd = new XoaNhanVienWindow(); wd.ShowDialog(); });

            StaffList = new List<StaffDTO>() { };
            DateTime x = new DateTime(122);
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002,12,24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002, 12, 24)));
            StaffList.Add(new StaffDTO(20520990, "Kiều Bá Dương", "Nam", new DateTime(2021, 10, 20), "01683725259", "Nhân viên", new DateTime(2002,12,24)));
        }
    }
}
