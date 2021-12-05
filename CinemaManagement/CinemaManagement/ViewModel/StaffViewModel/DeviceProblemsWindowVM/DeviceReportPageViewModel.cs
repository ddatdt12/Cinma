using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.DeviceProblemsWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public class DeviceReportPageViewModel : BaseViewModel
    {
        #region Command cho DeviceReportPage
        public ICommand LoadPageCommand { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        #endregion

        #region Command cho AddError, View và EditError
        public ICommand UploadImageCM { get; set; }
        public ICommand CancelCM { get; set; }
        public ICommand ConfirmCM { get; set; }
        #endregion

        #region Biến Binding cho DeviceReportPage
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
        #endregion

        #region Biến cho Add, View và Edit
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
        #endregion

        bool IsAddingError;
        bool IsEditError;
        bool IsViewError;
        MessageBoxCustom Message;
        public DeviceReportPageViewModel()
        {


            #region Command cho Report
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadListError();
            });

            OpenAddErrorCommand = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                IsAddingError = true;
                //Khởi tạo giá trị
                InitializationAdding();
                //Hiển thị window thêm lỗi
                AddError window = new AddError();
                window.ShowDialog();

            });

            LoadPageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                InitializationEditing();
                if (SelectedItem.Status == "Chờ tiếp nhận")
                {
                    IsEditError = true;
                    //Mở window edit
                    EditError window = new EditError();
                    window.ShowDialog();
                    FilterListError();
                }
                else
                {
                    IsViewError = true;
                    //Mở window view
                    ViewError window = new ViewError();
                    window.ShowDialog();
                }
            });

            FilterListErrorCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    FilterListError();
                }
            });
            #endregion

            #region Command cho Add, Edit và View
            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (!IsViewError)
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Title = "Load Image";
                    openFile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(openFile.FileName))
                            ImgSource = openFile.FileName;
                        return;
                    }
                }
                else
                {
                    Message = new MessageBoxCustom("Cảnh báo", "Không thể thay đổi thông tin ở chế độ xem!", MessageType.Error, MessageButtons.OK);
                    Message.ShowDialog();
                }
            });

            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    IsViewError = IsEditError = IsAddingError = false;
                    p.Close();
                }
            });

            ConfirmCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (IsValidData())
                {
                    if (IsCheckStaff())
                    {
                        SaveData();
                        if (IsEditError)
                        {
                            (bool issuc, string mess) = TroubleService.Ins.UpdateTroubleInfo(ErrorDevice);

                            if (issuc)
                            {
                                Message = new MessageBoxCustom("Thông báo", mess, MessageType.Success, MessageButtons.OK);
                                IsEditError = false;
                                GetData();
                            }
                            else
                            {
                                Message = new MessageBoxCustom("Thông báo", mess, MessageType.Error, MessageButtons.OK);
                            }
                        }
                        else
                        {
                            (bool issuc, string mess, TroubleDTO newtrouble) = TroubleService.Ins.CreateNewTrouble(ErrorDevice);

                            if (issuc)
                            {
                                GetAllError.Add(newtrouble);
                                Message = new MessageBoxCustom("Thông báo", mess, MessageType.Success, MessageButtons.OK);
                                IsAddingError = false;
                            }
                            else
                            {
                                Message = new MessageBoxCustom("Thông báo", mess, MessageType.Error, MessageButtons.OK);
                            }
                        }
                        FilterListError();
                        Message.ShowDialog();
                        p.Close();
                    }
                    else
                    {
                        Message = new MessageBoxCustom("Lỗi", "Nhân viên không tồn tại. Vui lòng kiểm tra lại!", MessageType.Error, MessageButtons.OK);
                        Message.ShowDialog();
                    }
                }
                else
                {
                    Message = new MessageBoxCustom("Thông báo", "Vui lòng nhập đủ thông tin!", MessageType.Error, MessageButtons.OK);
                    Message.ShowDialog();
                }
            });

            #endregion
        }

        #region Function cho Report
        public void LoadListError()
        {
            //Khởi tạo danh sách lỗi và giá trị biến tham số
            ListError = new ObservableCollection<TroubleDTO>();
            GetAllError = new ObservableCollection<TroubleDTO>();
            ErrorDevice = new TroubleDTO();
            IsAddingError = false;
            IsEditError = false;
            IsViewError = false;

            //
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
            //GetAllError = new ObservableCollection<TroubleDTO>(TroubleService.Ins.GetAllTrouble());
            ListError = new ObservableCollection<TroubleDTO>(GetAllError);
        }
        private Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
        #endregion

        #region Function cho Add
        public void RefreshAddPage()
        {
            Title = null;
            StaffId = null;
            StaffName = null;
            Status = "Chờ tiếp nhận";
            SubmittedAt = DateTime.Now;
            Description = null;
            ImgSource = null;
        } //Gán lại giá trị mặc định trước khi thêm mới lỗi
        public void InitializationAdding()
        {
            RefreshAddPage();
            ErrorDevice = new TroubleDTO();
            ListStaff = new List<StaffDTO>();
            SubmittedAt = DateTime.Now;

            //Gán danh sách nhân viên
            LoadListStaff();
        } //Khởi tạo cho biến và danh sách nhân viên

        public void LoadListStaff()
        {
            ListStaff = new List<StaffDTO>(StaffService.Ins.GetAllStaff());
        }
        public bool IsCheckStaff()
        {
            foreach (var temp in ListStaff)
            {
                if (string.Compare(StaffId, temp.Id, true) == 0 && string.Compare(StaffName, temp.Name, true) == 0)
                {
                    StaffId = temp.Id;
                    StaffName = temp.Name;
                    return true;
                }
            }
            return false;
        }   //Kiểm tra nhân viên trong danh sách
        public bool IsValidData()
        {

            return !string.IsNullOrEmpty(StaffId) && !string.IsNullOrEmpty(Title)
                     && !string.IsNullOrEmpty(Description);
                    //&& !string.IsNullOrEmpty(ImgSource);
        }   //Kiểm tra giá trị đã nhập để thêm vào danh sách lỗi
        public void SaveData()
        {
            if (IsEditError) ErrorDevice = SelectedItem;
            //TroubleService.Ins.CreateNewTrouble(ErrorDevice);
            ErrorDevice.Title = Title;
            ErrorDevice.StaffId = StaffId;
            ErrorDevice.StaffName = StaffName;
            ErrorDevice.Status = Status;
            ErrorDevice.SubmittedAt = SubmittedAt;
            ErrorDevice.Description = Description;
            ErrorDevice.Image = ImgSource;

        }   //Lưu giá trị đã nhập vào biến ErrorDevice

        #endregion

        #region Function cho Edit
        public void RefreshEditPage()
        {
            Title = SelectedItem.Title;
            StaffId = SelectedItem.StaffId;
            StaffName = SelectedItem.StaffName;
            Status = SelectedItem.Status;
            SubmittedAt = SelectedItem.SubmittedAt;
            Description = SelectedItem.Description;
            ImgSource = SelectedItem.Image;
            Level = SelectedItem.Level;
            StartDate = SelectedItem.StartDate;
            FinishDate = SelectedItem.FinishDate;
            RepairCostStr = SelectedItem.RepairCostStr;
        } //Refresh giá trị cho page edit
        public void InitializationEditing()
        {
            RefreshEditPage();
            ListStaff = new List<StaffDTO>();

            //Gán danh sách nhân viên
            LoadListStaff();
        } //Khởi tạo cho biến và danh sách nhân viên

        #endregion
    }
}
