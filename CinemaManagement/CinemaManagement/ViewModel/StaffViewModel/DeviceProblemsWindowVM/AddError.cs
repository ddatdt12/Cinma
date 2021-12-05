using CinemaManagement.DTOs;
using CinemaManagement.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public partial class DeviceReportPageViewModel : BaseViewModel
    {
        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }
        public ICommand UploadImageCM { get; set; }
        public ICommand ConfirmCM { get; set; }


        //public AddDeviceProblemPageViewModel()
        //{
        //    //Khởi tạo
        //    Initialization();
        //    UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
        //    {
        //        OpenFileDialog openFile = new OpenFileDialog();
        //        openFile.Title = "Load Image";
        //        openFile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
        //        if (openFile.ShowDialog() == DialogResult.OK)
        //        {
        //            if (File.Exists(openFile.FileName))
        //                ImgSource = openFile.FileName;
        //            return;
        //        }
        //    });

        //    CancelCM = new RelayCommand<object>((p) => { return true; }, (p) =>
        //    {
        //        if (p != null)
        //        {

        //        }
        //    });

        //    ConfirmCM = new RelayCommand<object>((p) => { return true; }, (p) =>
        //    {
        //        if (IsValidData())
        //        {
        //            if (IsCheckStaff())
        //            {
        //                SaveData();
        //                //Wtrite code here

        //                Message = new MessageBoxCustom("Thông báo", "Lưu thông tin thành công!", MessageType.Info, MessageButtons.OK);
        //                Message.ShowDialog();
        //            }
        //            else
        //            {
        //                Message = new MessageBoxCustom("Lỗi", "Nhân viên không tồn tại. Vui lòng kiểm tra lại!", MessageType.Error, MessageButtons.OK);
        //                Message.ShowDialog();
        //            }
        //        }
        //        else
        //        {
        //            Message = new MessageBoxCustom("Thông báo", "Vui lòng nhập đủ thông tin!", MessageType.Error, MessageButtons.OK);
        //            Message.ShowDialog();
        //        }
        //    });
        //}

        //public void Refresh()
        //{
        //    Title = null;
        //    StaffId = null;
        //    StaffName = null;
        //    Status = "Chờ tiếp nhận";
        //    SubmittedAt = DateTime.Now;
        //    Description = null;
        //    ImgSource = null;
        //}

        //public void Initialization()
        //{
        //    Refresh();
        //    ErrorDevice = new TroubleDTO();
        //    ListStaff = new List<StaffDTO>();
        //    GetCurrentDate = DateTime.Today;
        //    //Gán danh sách nhân viên
        //    LoadListStaff();
        //}

        //public void SaveData()
        //{
        //    ErrorDevice.Title = Title;
        //    ErrorDevice.StaffId = StaffId;
        //    ErrorDevice.StaffName = StaffName;
        //    ErrorDevice.Status = Status;
        //    ErrorDevice.SubmittedAt = SubmittedAt;
        //    ErrorDevice.Description = Description;
        //    ErrorDevice.Image = ImgSource;
        //}

        //public bool IsCheckStaff()
        //{
        //    foreach (var temp in ListStaff)
        //    {
        //        if (StaffId == temp.Id && StaffName == temp.Name) return true;
        //    }
        //    return false;
        //}

        //public bool IsValidData()
        //{

        //    return !string.IsNullOrEmpty(StaffName) && !string.IsNullOrEmpty(StaffId) && !string.IsNullOrEmpty(Title)
        //            && !string.IsNullOrEmpty(SubmittedAt.ToString()) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(Description)
        //            && !string.IsNullOrEmpty(ImgSource);
        //}

        //public void LoadListStaff()
        //{
        //    ListStaff.Add(new StaffDTO() { Id = "NV01", Name = "Kiều Bá Dương" });
        //    ListStaff.Add(new StaffDTO() { Id = "NV02", Name = "Trần Đình Khôi" });
        //    ListStaff.Add(new StaffDTO() { Id = "NV03", Name = "Huỳnh Trung Thảo" });
        //    ListStaff.Add(new StaffDTO() { Id = "NV04", Name = "Lê Hải Phong" });
        //    ListStaff.Add(new StaffDTO() { Id = "NV05", Name = "Đỗ Thành Đạt" });
        //}
    }
}
