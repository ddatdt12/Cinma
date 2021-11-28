using System.Windows;
using CinemaManagement.Models.Services;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void DeleteStaff(Window p)
        {
            (bool successDeleteStaff, string messageFromDeleteStaff) = StaffService.Ins.DeleteStaff(SelectedItem.Id);
            if (successDeleteStaff)
            {
                MaskName.Visibility = Visibility.Collapsed;
                p.Close();
                LoadStaffListView(Utils.Operation.DELETE);
            }
            MessageBox.Show(messageFromDeleteStaff);
        }
    }
}