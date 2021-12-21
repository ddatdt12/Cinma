using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        private string _Mail;
        public string Mail
        {
            get { return _Mail; }
            set { _Mail = value; OnPropertyChanged(); }
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
        public ICommand OpenChangePassCommand { get; set; }

        public ICommand MouseMoveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }





        private StaffDTO _SelectedItem;
        public StaffDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        public static Grid MaskName { get; set; }


        public StaffManagementViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    StaffList = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });
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

            AddStaffCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await AddStaff(p);
                    IsSaving = false;
                });
            EditStaffCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await EditStaff(p);
                    IsSaving = false;
                });
            ChangePassCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await ChangePass(p);
                    IsSaving = false;
                });
            DeleteStaffCommand = new RelayCommand<Window>((p) => { return true; },
                 async (p) =>
                {
                    MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", "Bạn có chắc muốn xoá nhân viên này không?", MessageType.Warning, MessageButtons.YesNo);
                    result.ShowDialog();

                    if (result.DialogResult == true)
                    {
                        (bool successDeleteStaff, string messageFromDeleteStaff) = await StaffService.Ins.DeleteStaff(SelectedItem.Id);
                        if (successDeleteStaff)
                        {
                            LoadStaffListView(Utils.Operation.DELETE);
                            MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromDeleteStaff, MessageType.Success, MessageButtons.OK);
                            mb.ShowDialog();
                        }
                        else
                        {
                            MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromDeleteStaff, MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                        }
                    }
                });

            OpenAddStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ThemNhanVienWindow wd = new ThemNhanVienWindow();
                    ResetData();
                    MaskName.Visibility = Visibility.Visible;
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
                    wd._Mail.Text = SelectedItem.Email;

                    Fullname = SelectedItem.Name;
                    Phone = SelectedItem.PhoneNumber;
                    TaiKhoan = SelectedItem.Username;
                    Mail = SelectedItem.Email;

                    MaskName.Visibility = Visibility.Visible;
                    wd.ShowDialog();
                });

            OpenChangePassCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DoiMatKhau wd = new DoiMatKhau();
                MatKhau = null;
                RePass = null;
                wd.ShowDialog();
            });

            CloseCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;

                if (w != null)
                {
                    if (w is DoiMatKhau)
                    {
                        w.Close();
                        return;
                    }
                    MaskName.Visibility = Visibility.Collapsed;
                    w.Close();
                }
            }
            );
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });

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
            Mail = null;
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
                if (string.IsNullOrEmpty(MatKhau))
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
