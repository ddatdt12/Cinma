using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
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
    public partial class StaffManagementViewModel: BaseViewModel
    {
        public void AddStaff(Window p)
        {
            if (Fullname != null && Gender != null && StartDate != null && Born != null && Phone != null && Role != null && TaiKhoan != null)
            {
                DateTime x = DateTime.Today;
                StaffDTO staff = new StaffDTO();
                staff.Name = Fullname;
                staff.Gender = Gender.Content.ToString();
                staff.BirthDate = Born;
                staff.PhoneNumber = Phone;
                staff.Role = Role.Content.ToString();
                staff.StartingDate = StartDate;
                staff.Username = TaiKhoan;
                staff.Password = MatKhau;
                StaffList.Add(staff);
                int value;
                if (int.TryParse(Phone, out value))
                {
                    if (staff.Password == RePass)
                    {
                        (bool successAddStaff, string messageFromAddStaff) = StaffService.Ins.AddStaff(staff);

                        if (successAddStaff)
                        {
                            p.Close();
                            StaffList.Add(staff);
                            ReloadStaffListView();
                        }
                        MessageBox.Show(messageFromAddStaff);
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu và mật khẩu nhập lại không trùng khớp!");
                    }
                }
                else
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!");
                }
            }
            else
            {
                MessageBox.Show("Chưa đủ thông tin để thêm!");
            }
        }
    }
}
