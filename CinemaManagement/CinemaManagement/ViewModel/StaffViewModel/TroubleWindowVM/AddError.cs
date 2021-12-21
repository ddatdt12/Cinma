using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.TroubleWindow;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : BaseViewModel
    {
        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }
        public ICommand SaveErrorCM { get; set; }

        public async Task SaveErrorFunc(AddError p)
        {
            if (filepath != null && IsValidData())
            {
                string troubleImage = await CloudinaryService.Ins.UploadImage(filepath);
                if (troubleImage is null)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", MessageType.Error, MessageButtons.OK);
                    return;
                }

                TroubleDTO trouble = new TroubleDTO
                {
                    Title = Title,
                    Level = Level.Content.ToString(),
                    Description = Description,
                    Image = troubleImage,
                    StaffId = MainStaffViewModel.CurrentStaff.Id,
                };

                (bool successAddtrouble, string messageFromAddtrouble, TroubleDTO newtrouble) = await TroubleService.Ins.CreateNewTrouble(trouble);

                if (successAddtrouble)
                {
                    IsSaving = false;
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Thêm sự cố thành công", MessageType.Success, MessageButtons.OK);
                    GetAllError = new System.Collections.ObjectModel.ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());
                    ListError = new System.Collections.ObjectModel.ObservableCollection<TroubleDTO>(GetAllError);
                    MaskName.Visibility = Visibility.Collapsed;
                    mb.ShowDialog();
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đủ thông tin!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }
}
