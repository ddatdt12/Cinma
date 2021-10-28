using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CinemaManagement.Models.Services;
using CinemaManagement.ViewModel;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void DeleteStaff(Window p)
        {
            (bool successDeleteStaff, string messageFromDeleteStaff) = StaffService.Ins.DeleteStaff(SelectedItem.Id);
            if (successDeleteStaff)
            {
                p.Close();
                ReloadStaffListView();
            }
            MessageBox.Show(messageFromDeleteStaff);
        }
    }
}
