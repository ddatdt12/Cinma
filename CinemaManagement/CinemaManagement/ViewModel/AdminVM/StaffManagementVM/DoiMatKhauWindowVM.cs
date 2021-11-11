using CinemaManagement.Models.Services;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public void ChangePass(Window p)
        {

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE_PASSWORD);
            if (isValid)
            {
                (bool updatePassSuccesss, string message) = StaffService.Ins.UpdatePassword(SelectedItem.Id, MatKhau);
                if (updatePassSuccesss)
                    p.Close();
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }

}
