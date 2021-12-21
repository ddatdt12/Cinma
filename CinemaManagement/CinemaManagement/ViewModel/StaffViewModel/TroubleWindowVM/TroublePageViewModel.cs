using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Staff.TroubleWindow;
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

namespace CinemaManagement.ViewModel.StaffViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : BaseViewModel
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

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }


        public ICommand CancelCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        string filepath;
        bool IsImageChanged = false;

        public static Grid MaskName { get; set; }
        public TroublePageViewModel()
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
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await LoadListError();
                await LoadListStaff();
                IsLoading = false;

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
                    MaskName.Visibility = Visibility.Visible;
                    ViewError w = new ViewError();
                    w.ShowDialog();
                    return;
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
                MaskName.Visibility = Visibility.Visible;
                w1.StaffName.Text = MainStaffViewModel.CurrentStaff.Name;
                w1.ShowDialog();
            });
            SaveErrorCM = new RelayCommand<AddError>((p) => { if (IsSaving) return false; return true; }, async (p) =>
             {
                 IsSaving = true;
                 await SaveErrorFunc(p);
                 IsSaving = false;
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
                    LoadImage();
                    return;
                }
                IsImageChanged = false;

            });
            LoadEditErrorCM = new RelayCommand<EditError>((p) => { return true; }, (p) =>
            {
                EditError w1 = new EditError();
                LoadEditError(w1);
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            UpdateErrorCM = new RelayCommand<EditError>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateErrorFunc(p);
                isSaving = false;
            });

            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            CloseCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
             {
                 MaskName.Visibility = Visibility.Collapsed;
                 SelectedItem = null;
                 p.Close();
             });
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
        }

        public async Task LoadListError()
        {
            //Khởi tạo danh sách lỗi và giá trị biến tham số
            ListError = new ObservableCollection<TroubleDTO>();
            GetAllError = new ObservableCollection<TroubleDTO>();
            ErrorDevice = new TroubleDTO();
            IsImageChanged = false;

            //Lấy dữ liệu cho ListError
            await GetData();
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
        public async Task GetData()
        {
            GetAllError = new ObservableCollection<TroubleDTO>(await Task.Run(() => TroubleService.Ins.GetAllTrouble()));
            ListError = new ObservableCollection<TroubleDTO>(GetAllError);
        }
        public void RenewWindowData()
        {
            Title = null;
            Description = null;
            ImageSource = null;
            Level = null;
            filepath = null;
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
        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
    }
}
