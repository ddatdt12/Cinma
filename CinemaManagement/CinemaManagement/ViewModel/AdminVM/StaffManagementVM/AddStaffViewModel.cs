using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Windows;
using CinemaManagement.Utils;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void AddStaff(Window p)
        {

            (bool isValid, string error) = IsValidData(Operation.CREATE);

            if (isValid)
            {
                StaffDTO staff = new StaffDTO();
                staff.Name = Fullname;
                staff.Gender = Gender.Content.ToString();
                staff.BirthDate = Born;
                staff.PhoneNumber = Phone;
                staff.Role = Role.Content.ToString();
                staff.StartingDate = StartDate;
                staff.Username = TaiKhoan;
                staff.Password = MatKhau;


                (bool successAddStaff, string messageFromAddStaff, StaffDTO newStaff) = StaffService.Ins.AddStaff(staff);

                if (successAddStaff)
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                    LoadStaffListView(Operation.CREATE, newStaff);
                }
                MessageBox.Show(messageFromAddStaff);

            }
            else
            {
                MessageBox.Show(error);
            }
        }
        private (bool, string) ValidateAge(DateTime birthDate)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthDate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthDate.DayOfYear > today.DayOfYear) age--;

            if (age < 18) return (false, "Nhân viên chưa đủ 18 tuổi!");
            return (true, null);
        }
    }
}
