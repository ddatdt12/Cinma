using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public async Task ChangePass(Window p)
        {

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE_PASSWORD);

            if (isValid)
            {
                (bool updatePassSuccesss, string message) = await StaffService.Ins.UpdatePassword(SelectedItem.Id, MatKhau);
                if (updatePassSuccesss)
                {
                    p.Close();
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", message, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", message, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", error, MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }

}
