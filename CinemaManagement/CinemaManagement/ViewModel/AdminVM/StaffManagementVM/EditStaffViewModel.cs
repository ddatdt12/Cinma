using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public  partial class StaffManagementViewModel: BaseViewModel
    {
        public void EditStaff(SuaNhanVienWindow p)
        {
            Fullname = SelectedItem.Name;
            Phone = SelectedItem.PhoneNumber;
            TaiKhoan = SelectedItem.Username;
            MatKhau = SelectedItem.Password;
            if (Fullname != null && Gender != null && StartDate != null && Born != null && Phone != null && Role != null && TaiKhoan != null)
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
                staff.Password = MatKhau;

                int value;
                if (int.TryParse(Phone, out value))
                {
                    (bool successAddStaff, string messageFromAddStaff) = StaffService.Ins.UpdateStaff(staff);

                    if (successAddStaff)
                    {
                        p.Close();
                        ReloadStaffListView();
                    }
                    MessageBox.Show(messageFromAddStaff);
                }
                else
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!");
                }
            }
            else
            {
                MessageBox.Show("Thông tin nhân viên bị thiếu!");
            }
        }
    }
}
