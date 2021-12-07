using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.DeviceProblemsWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public partial class DeviceReportPageViewModel : BaseViewModel
    {


        public ICommand LoadEditErrorCM { get; set; }
        public ICommand UpdateErrorCM { get; set; }

        private string troubleID;
        public string TroubleID
        {
            get { return troubleID; }
            set { troubleID = value; }
        }

        public void LoadEditError(EditError w1)
        {

            IsImageChanged = false;
            imgfullname = SelectedItem.Image;
            Title = SelectedItem.Title;
            w1.staffname.Text = SelectedItem.StaffName;
            w1.cbxStatusError.Text = SelectedItem.Status;
            w1.submitdate.Text = SelectedItem.SubmittedAt.ToShortDateString();
            Level.Content = SelectedItem.Level;
            Description = SelectedItem.Description;
            TroubleID = SelectedItem.Id;


            if (File.Exists(Helper.GetTroubleImgPath(SelectedItem.Image)) == true)
            {
                ImageSource = Helper.GetTroubleImageSource(SelectedItem.Image);
            }
            else
            {
                w1.FrameImage.Source = Helper.GetTroubleImageSource("null.jpg");
            }
        }
        public async Task UpdateErrorFunc(EditError p)
        {
            if (TroubleID != null && IsValidData())
            {

                TroubleDTO tb = new TroubleDTO
                {
                    Id = TroubleID,
                    Title = Title,
                    Level = Level.Content.ToString(),
                    Description = Description,
                    Image = imgfullname,
                    StaffId = "NV002",
                };

                if (IsImageChanged)
                {
                    imgName = Helper.CreateImageName(Title);
                    imgfullname = Helper.CreateImageFullName(imgName, extension);
                    tb.Image = imgfullname;
                }
                else
                {
                    filepath = Helper.GetTroubleImgPath(SelectedItem.Image);
                    tb.Image = imgfullname = Helper.CreateImageFullName(Helper.CreateImageName(tb.Title), SelectedItem.Image.Split('.')[1]);
                }

                (bool successUpdateTB, string messageFromUpdateTB) =await TroubleService.Ins.UpdateTroubleInfo(tb);

                if (successUpdateTB)
                {
                    SaveImgToApp();
                    MessageBoxCustom mb = new MessageBoxCustom("", "Cập nhật thành công!", MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    await GetData();

                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Vui lòng nhập đủ thông tin", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }

    }
}
