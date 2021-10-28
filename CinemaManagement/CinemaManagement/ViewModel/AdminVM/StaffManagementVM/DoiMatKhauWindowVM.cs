using CinemaManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void ChangePass(Window p)
        {
            if (MK!=null && ReMK!=null)
            {
                if (MK == ReMK)
                {
                    MatKhau = MK;
                    SelectedItem.Password = MK;
                    p.Close();
                    MessageBox.Show("Sua mat khau thanh cong!");
                }
                else
                {
                    MessageBox.Show("Mật khẩu không trùng khớp!");
                }
            }
            else
            {
                MessageBox.Show("Mật khẩu không được để trống!");
            }
        }
    }

}
