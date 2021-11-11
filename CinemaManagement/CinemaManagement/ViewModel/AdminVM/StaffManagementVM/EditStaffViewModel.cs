using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void EditStaff(Window p)
        {
            //Fullname = SelectedItem.Name;
            //Phone = SelectedItem.PhoneNumber;
            //TaiKhoan = SelectedItem.Username;
            MatKhau = SelectedItem.Password;

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE);
            if (isValid)
            {
                StaffDTO staff = new StaffDTO();
                staff.Id = SelectedItem.Id;
                staff.Name = Fullname;
                staff.Gender = Gender.Content.ToString();
                staff.BirthDate = Born;
                staff.PhoneNumber = Phone;
                staff.Role = Role.Content.ToString();
                staff.StartingDate = StartDate;
                staff.Username = TaiKhoan;
                (bool successUpdateStaff, string messageFromUpdateStaff) = StaffService.Ins.UpdateStaff(staff);

                if (successUpdateStaff)
                {
                    p.Close();
                    LoadStaffListView(Utils.Operation.UPDATE, staff);
                }
                MessageBox.Show(messageFromUpdateStaff);
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }
}
