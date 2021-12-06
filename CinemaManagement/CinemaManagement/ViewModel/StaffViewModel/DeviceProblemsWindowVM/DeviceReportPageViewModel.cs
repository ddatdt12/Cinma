using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.DeviceProblemsWindow;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public partial class DeviceReportPageViewModel : BaseViewModel
    {
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

        private ObservableCollection<StaffDTO> _ListStaff;
        public ObservableCollection<StaffDTO> ListStaff
        {
            get => _ListStaff;
            set { _ListStaff = value; OnPropertyChanged(); }
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set { _Title = value; OnPropertyChanged(); }
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

        private ComboBoxItem _Level;
        public ComboBoxItem Level
        {
            get => _Level;
            set { _Level = value; OnPropertyChanged(); }
        }


        public ICommand CancelCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand UploadImageCM { get; set; }

        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;
        string oldErrorName;
        bool IsImageChanged = false;
        bool IsAddingError = false;

        public static Grid MaskName { get; set; }
        public static StaffDTO CurrentStaff { get; set; }
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
                LoadListStaff();

            });
            FilterListErrorCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
            {
                FilterListError();
            });
            LoadDetailWindowCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem is null) return;

                if (SelectedItem.Status == Utils.STATUS.IN_PROGRESS || SelectedItem.Status == Utils.STATUS.DONE)
                {
                    ViewError w = new ViewError();
                    w.ShowDialog();
                }
                if (SelectedItem.Status == Utils.STATUS.CANCLE)
                {
                    return;
                }
            });
            OpenAddErrorCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                AddError w1 = new AddError();
                IsAddingError = true;
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            SaveErrorCM = new RelayCommand<AddError>((p) => { return true; }, (p) =>
            {
                SaveErrorFunc(p);
            });
            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    extension = openfile.SafeFileName.Split('.')[1];
                    LoadImage();
                    return;
                }
                IsImageChanged = false;

            });
            LoadEditErrorCM = new RelayCommand<EditError>((p) => { return true; }, (p) =>
            {
                oldErrorName = Title;
                EditError w1 = new EditError();
                LoadEditError(w1);
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            UpdateErrorCM = new RelayCommand<EditError>((p) => { return true; }, (p) =>
            {
                UpdateErrorFunc(p);
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
            GetAllError = new ObservableCollection<TroubleDTO>(TroubleService.Ins.GetAllTrouble());
            ListError = new ObservableCollection<TroubleDTO>(GetAllError);
        }
        public void RenewWindowData()
        {
            Title = null;
            Description = null;
            ImageSource = null;
            Level = null;
        }

        public async Task LoadListStaff()
        {
            ListStaff = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(Title)
                     && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Level.Content.ToString());
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        public void SaveImgToApp()
        {
            try
            {
                if (IsAddingError)
                {
                    appPath = Helper.GetTroubleImgPath(imgfullname);
                    File.Copy(filepath, appPath, true);
                    return;
                }
                if (imgfullname != SelectedItem.Image)
                {
                    appPath = Helper.GetTroubleImgPath(imgfullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetTroubleImgPath(SelectedItem.Image)))
                        File.Delete(Helper.GetTroubleImgPath(SelectedItem.Image));
                }
                else
                {
                    string temp_name = imgfullname.Split('.')[0] + "temp";
                    string temp_ex = imgfullname.Split('.')[1];
                    string temp_fullname = Helper.CreateImageFullName(temp_name, temp_ex);

                    appPath = Helper.GetTroubleImgPath(temp_fullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetTroubleImgPath(imgfullname)))
                        File.Delete(Helper.GetTroubleImgPath(imgfullname));
                    appPath = Helper.GetTroubleImgPath(imgfullname);
                    filepath = Helper.GetTroubleImgPath(temp_fullname);
                    File.Copy(filepath, appPath, true);
                    if (File.Exists(Helper.GetTroubleImgPath(temp_fullname)))
                        File.Delete(Helper.GetTroubleImgPath(temp_fullname));

                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }
    }
}
