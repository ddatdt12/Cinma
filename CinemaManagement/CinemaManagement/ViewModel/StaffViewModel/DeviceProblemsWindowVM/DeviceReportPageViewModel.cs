using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.DeviceProblemsWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public partial class DeviceReportPageViewModel : BaseViewModel
    {
        public ICommand CancelCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand MaskNameCM { get; set; }


        public static ObservableCollection<TroubleDTO> GetAllError { get; set; }

        private ObservableCollection<TroubleDTO> _ListError;
        public ObservableCollection<TroubleDTO> ListError
        {
            get => _ListError;
            set { _ListError = value; OnPropertyChanged(); }
        }

        private TroubleDTO _SelectedItem;
        public TroubleDTO SelectedItem
        {
            get => _SelectedItem;
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _ItemViewMode;
        public ComboBoxItem ItemViewMode
        {
            get => _ItemViewMode;
            set { _ItemViewMode = value; OnPropertyChanged(); }
        }


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

        private string _Status;
        public string Status
        {
            get => _Status;
            set { _Status = value; OnPropertyChanged(); }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged(); }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private string _Level;
        public string Level
        {
            get => _Level;
            set { _Level = value; OnPropertyChanged(); }
        }

        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;
        string oldErrorName;
        bool IsImageChanged = false;
        bool IsAddingError = false;
        MessageBoxCustom Message;
        public static Grid MaskName { get; set; }
        public static StaffDTO CurrentStaff{ get; set; }
        public DeviceReportPageViewModel()
        {
            GetCurrentDate = System.DateTime.Today;
            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    if (p != null)
                    {
                        p.Close();
                        MaskName.Visibility = Visibility.Collapsed;
                    }
                });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadListError();
            });
            FilterListErrorCommand = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                FilterListError();
            });
            LoadDetailWindowCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem is null) return;

                if (SelectedItem.Status == Utils.STATUS.WAITING)
                {

                }
                if (SelectedItem.Status == Utils.STATUS.IN_PROGRESS)
                {

                }
                if (SelectedItem.Status == Utils.STATUS.DONE)
                {

                }
            });
            OpenAddErrorCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                Window w1 = new AddError();
                IsAddingError = true;
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });




            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
        }

        public void LoadListError()
        {
            //Khởi tạo danh sách lỗi và giá trị biến tham số
            ListError = new ObservableCollection<TroubleDTO>();
            GetAllError = new ObservableCollection<TroubleDTO>();
            ErrorDevice = new TroubleDTO();
            IsImageChanged = false;

            GetAllError = new ObservableCollection<TroubleDTO>(TroubleService.Ins.GetAllTrouble());

            //Lấy dữ liệu cho ListError
            GetData();
        }
        public void FilterListError()
        {
            ListError.Clear();
            if (ItemViewMode.Content.ToString() == "Toàn bộ")
            {
                for (int i = 0; i < GetAllError.Count; ++i)
                {
                    ListError.Add(GetAllError[i]);
                }
            }
            else
            {
                for (int i = 0; i < GetAllError.Count; ++i)
                {
                    if (GetAllError[i].Status == ItemViewMode.Content.ToString())
                    {
                        ListError.Add(GetAllError[i]);
                    }
                }
            }
        }
        public void GetData()
        {
            ListError = new ObservableCollection<TroubleDTO>(GetAllError);
        }
        public void RenewWindowData()
        {
            Title = null;
            StaffId = null;
            Description = null;
            ImageSource = null;
            Level = null;
        }
        public void RefreshAddPage()
        {
            Title = null;
            StaffId = null;
            Status = "Chờ tiếp nhận";
            Description = null;
        } //Gán lại giá trị mặc định trước khi thêm mới lỗi
        public void InitializationAdding()
        {
            RefreshAddPage();
            ErrorDevice = new TroubleDTO();
            ListStaff = new List<StaffDTO>();

            //Gán danh sách nhân viên
            LoadListStaff();
        } //Khởi tạo cho biến và danh sách nhân viên

        public void LoadListStaff()
        {
            ListStaff = new List<StaffDTO>(StaffService.Ins.GetAllStaff());
        }
        public bool IsValidData()
        {

            return !string.IsNullOrEmpty(StaffId) && !string.IsNullOrEmpty(Title)
                     && !string.IsNullOrEmpty(Description);
            //&& !string.IsNullOrEmpty(ImgSource);
        }   //Kiểm tra giá trị đã nhập để thêm vào danh sách lỗi
    }
}
