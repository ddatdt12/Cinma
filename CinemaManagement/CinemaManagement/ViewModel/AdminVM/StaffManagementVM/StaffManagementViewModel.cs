using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        ListView listView;

        #region Biến lưu dữ liệu thêm

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { _Fullname = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Gender;
        public ComboBoxItem Gender
        {
            get { return _Gender; }
            set { _Gender = value; OnPropertyChanged(); }
        }

        private Nullable<System.DateTime> _Born;
        public Nullable<System.DateTime> Born
        {
            get { return _Born; }
            set { _Born = value; OnPropertyChanged(); }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Role;
        public ComboBoxItem Role
        {
            get { return _Role; }
            set { _Role = value; OnPropertyChanged(); }
        }

        private Nullable<System.DateTime> _StartDate;
        public Nullable<System.DateTime> StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged(); }
        }

        private string _TaiKhoan;
        public string TaiKhoan
        {
            get { return _TaiKhoan; }
            set { _TaiKhoan = value; OnPropertyChanged(); }
        }

        private string _MatKhau;
        public string MatKhau
        {
            get { return _MatKhau; }
            set { _MatKhau = value; OnPropertyChanged(); }
        }

        private string _RePass;
        public string RePass
        {
            get { return _RePass; }
            set { _RePass = value; OnPropertyChanged(); }
        }


        #endregion

        private ObservableCollection<StaffDTO> _staffList;
        public ObservableCollection<StaffDTO> StaffList
        {
            get => _staffList;
            set
            {
                _staffList = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetListViewCommand { get; set; }
        public ICommand GetPasswordCommand { get; set; }
        public ICommand GetRePasswordCommand { get; set; }



        public ICommand AddStaffCommand { get; set; }
        public ICommand EditStaffCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }
        public ICommand DeleteStaffCommand { get; set; }

        public ICommand OpenAddStaffCommand { get; set; }
        public ICommand OpenEditStaffCommand { get; set; }
        public ICommand OpenDeleteStaffCommand { get; set; }
        public ICommand OpenChangePassCommand { get; set; }

        public ICommand MouseMoveCommand { get; set; }
        public ICommand CloseCommand { get; set; }





        private StaffDTO _SelectedItem;
        public StaffDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }


        public StaffManagementViewModel()
        {

            LoadStaffListView(Operation.READ);
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; },
                (p) =>
                {
                    listView = p;
                });
            GetPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) =>
                {
                    MatKhau = p.Password;
                });
            GetRePasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) =>
                {
                    RePass = p.Password;
                });

            AddStaffCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    AddStaff(p);
                });
            EditStaffCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    EditStaff(p);
                });
            ChangePassCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    ChangePass(p);
                });
            DeleteStaffCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    DeleteStaff(p);
                });

            OpenAddStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ThemNhanVienWindow wd = new ThemNhanVienWindow();
                    ResetData();
                    wd.ShowDialog();

                });
            OpenEditStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    SuaNhanVienWindow wd = new SuaNhanVienWindow();
                    ResetData();
                    wd._FullName.Text = SelectedItem.Name;
                    string x = SelectedItem.Gender;
                    if (x == "Nam")
                    {
                        wd.Gender.Text = SelectedItem.Gender;
                    }
                    else
                    {
                        wd.Gender.Text = "Nữ";
                    }
                    wd.Date.Text = SelectedItem.BirthDate.ToString();
                    wd._Phone.Text = SelectedItem.PhoneNumber.ToString();
                    wd.Role.Text = SelectedItem.Role;
                    wd.StartDate.Text = SelectedItem.StartingDate.ToString();
                    wd._TaiKhoan.Text = SelectedItem.Username;

                    Fullname = SelectedItem.Name;
                    //Gender.Content = SelectedItem.Gender;
                    //Born = (DateTime)SelectedItem.BirthDate;
                    Phone = SelectedItem.PhoneNumber;
                    //Role.Content = SelectedItem.Role;
                    //StartDate = (DateTime)SelectedItem.StartingDate;
                    TaiKhoan = SelectedItem.Username;

                    wd.ShowDialog();
                });

            OpenDeleteStaffCommand = new RelayCommand<object>((p) => { return true; }, (p) => { XoaNhanVienWindow wd = new XoaNhanVienWindow(); wd.ShowDialog(); });

            OpenChangePassCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DoiMatKhau wd = new DoiMatKhau();
                MatKhau = null;
                RePass = null;
                wd.ShowDialog();
            });

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
            );

            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            }
           );
        }

        public void LoadStaffListView(Operation oper, StaffDTO staff = null)
        {

            switch (oper)
            {
                case Operation.READ:
                    try
                    {
                        StaffList = new ObservableCollection<StaffDTO>(StaffService.Ins.GetAllStaff());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Lỗi hệ thống " + e.Message);
                    }
                    break;
                case Operation.CREATE:
                    StaffList.Add(staff);
                    break;
                case Operation.UPDATE:
                    var movieFound = StaffList.FirstOrDefault(s => s.Id == staff.Id);
                    StaffList[StaffList.IndexOf(movieFound)] = staff;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < StaffList.Count; i++)
                    {
                        if (StaffList[i].Id == SelectedItem?.Id)
                        {
                            StaffList.Remove(StaffList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            //Ko cần này nhe
            //listView.Items.Refresh();
        }
        void ResetData()
        {
            StartDate = null;
            Fullname = null;
            Gender = null;
            Born = null;
            Role = null;
            Phone = null;
            TaiKhoan = null;
            MatKhau = null;
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
        private (bool valid, string error) IsValidData(Operation oper)
        {

            if (string.IsNullOrEmpty(Fullname) || Gender is null || StartDate is null || Born is null || Role is null || string.IsNullOrEmpty(TaiKhoan))
            {
                return (false, "Thông tin nhân viên thiếu! Vui lòng bổ sung");
            }

            if (oper == Operation.CREATE || oper == Operation.UPDATE_PASSWORD)
            {
                if (MatKhau is null)
                {
                    return (false, "Vui lòng nhập mật khẩu");
                }
                if (MatKhau != RePass)
                    return (false, "Mật khẩu và mật khẩu nhập lại không trùng khớp!");
            }

            (bool ageValid, string error) = ValidateAge((DateTime)Born);

            if (!ageValid)
            {
                return (false, error);
            }

            if (!Helper.IsPhoneNumber(Phone))
            {
                return (false, "Số điện thoại không hợp lệ");
            }

            return (true, null);
        }
    }
}
