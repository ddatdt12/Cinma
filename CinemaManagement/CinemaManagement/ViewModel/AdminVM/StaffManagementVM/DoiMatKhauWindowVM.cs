using CinemaManagement.Models.Services;
using CinemaManagement.Views;
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
                {
                    p.Close();
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("", error, MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }

}
