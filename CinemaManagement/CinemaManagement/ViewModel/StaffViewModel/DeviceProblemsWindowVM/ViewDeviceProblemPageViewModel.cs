using CinemaManagement.DTOs;
using CinemaManagement.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public class ViewDeviceProblemPageViewModel : BaseViewModel
    {


        public ICommand UploadImageCM { get; set; }
        public ICommand OkCM { get; set; }

        private TroubleDTO _ErrorDevice;
        public TroubleDTO ErrorDevice
        {
            get => _ErrorDevice;
            set { _ErrorDevice = value; OnPropertyChanged(); }
        }

        private List<StaffDTO> _ListStaff;
        public List<StaffDTO> ListStaff
        {
            get => _ListStaff;
            set { _ListStaff = value; }
        }

        private TroubleDTO _SelectedItem;
        public TroubleDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set { _Title = value; OnPropertyChanged(); }
        }

        private string _StaffId;
        public string StaffId
        {
            get => _StaffId;
            set { _StaffId = value; OnPropertyChanged(); }
        }

        private string _StaffName;
        public string StaffName
        {
            get => _StaffName;
            set { _StaffName = value; OnPropertyChanged(); }
        }

        private string _Status;
        public string Status
        {
            get => _Status;
            set { _Status = value; OnPropertyChanged(); }
        }

        private DateTime _SubmittedAt;
        public DateTime SubmittedAt
        {
            get => _SubmittedAt;
            set
            {
                _SubmittedAt = value;
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged(); }
        }

        private string _ImgSource;
        public string ImgSource
        {
            get => _ImgSource;
            set { _ImgSource = value; OnPropertyChanged(); }
        }

        private string _Level;
        public string Level
        {
            get => _Level;
            set { _Level = value; OnPropertyChanged(); }
        }

        private string _RepairCostStr;
        public string RepairCostStr
        {
            get => _RepairCostStr;
            set { _RepairCostStr = value; OnPropertyChanged(); }
        }

        private DateTime? _StartDate;
        public DateTime? StartDate
        {
            get => _StartDate;
            set { _StartDate = value; OnPropertyChanged(); }
        }

        private DateTime? _FinishDate;
        public DateTime? FinishDate
        {
            get => _FinishDate;
            set { _FinishDate = value; OnPropertyChanged(); }
        }

        public ViewDeviceProblemPageViewModel()
        {
            Refresh();

            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MessageBoxCustom Message = new MessageBoxCustom("Cảnh báo", "Không thể thay đổi thông tin ở chế độ xem!", MessageType.Error, MessageButtons.OK);
                Message.ShowDialog();
            });

            OkCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Write page turn command 
            });
        }

        public void Refresh()
        {
            Title = ErrorDevice.Title;
            StaffId = ErrorDevice.StaffId;
            StaffName = ErrorDevice.StaffName;
            Status = ErrorDevice.Status;
            SubmittedAt = ErrorDevice.SubmittedAt;
            Description = ErrorDevice.Description;
            ImgSource = ErrorDevice.Image;
            Level = ErrorDevice.Level;
            StartDate = ErrorDevice.StartDate;
            FinishDate = ErrorDevice.FinishDate;
            RepairCostStr = ErrorDevice.RepairCostStr;
        }
    }
}