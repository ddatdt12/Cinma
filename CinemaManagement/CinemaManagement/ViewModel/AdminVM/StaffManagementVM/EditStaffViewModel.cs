using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public async Task EditStaff(Window p)
        {
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
                (bool successUpdateStaff, string messageFromUpdateStaff) = await StaffService.Ins.UpdateStaff(staff);
                await LoadStaffListView(Utils.Operation.UPDATE, staff);

                if (successUpdateStaff)
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdateStaff, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdateStaff, MessageType.Error, MessageButtons.OK);
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
