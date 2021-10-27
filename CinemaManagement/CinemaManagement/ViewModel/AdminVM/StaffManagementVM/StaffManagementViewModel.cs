﻿using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.QuanLyNhanVienPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private DateTime _Born;
        public DateTime Born
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

        private DateTime _StartDate;
        public DateTime StartDate
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
            StartDate = DateTime.Today;
            Born = DateTime.Today;
            StaffList = new ObservableCollection<StaffDTO>(StaffService.Ins.GetAllStaff());
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; },
                (p) => {
                    listView = p;
                });
            GetPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) => {
                    MatKhau = p.Password;
                });
            GetRePasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) => {
                    RePass = p.Password;
                });

            AddStaffCommand = new RelayCommand<Window>((p) => { return true; }, 
                (p) => {
                    if (Fullname != null && Gender != null && StartDate != null && Born != null && Phone != null && Role != null && TaiKhoan != null)
                    {
                        DateTime x = DateTime.Today;
                        StaffDTO staff = new StaffDTO();
                        staff.Name = Fullname;
                        staff.Gender = Gender.Content.ToString();
                        staff.BirthDate = x;
                        staff.PhoneNumber = Phone;
                        staff.Role = Role.Content.ToString();
                        staff.StartingDate = x;
                        staff.Username = TaiKhoan;
                        staff.Password = MatKhau;

                        int value;
                        if (int.TryParse(Phone, out value))
                        {
                            if (MatKhau == RePass)
                            {
                                (bool successAddStaff, string messageFromAddStaff) = StaffService.Ins.AddStaff(staff);

                                if (successAddStaff)
                                {
                                    p.Close();
                                    StaffList.Add(staff);
                                    ReloadStaffListView();
                                }
                                MessageBox.Show(messageFromAddStaff);
                            }
                            else
                            {
                                MessageBox.Show("Mật khẩu và mật khẩu nhập lại không trùng khớp!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Số điện thoại không hợp lệ!");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Chưa đủ thông tin để thêm!");
                    }
                });
            EditStaffCommand = new RelayCommand<Window>((p) => { return true; },
                (p) => {
                    MessageBox.Show(SelectedItem.Gender);
                });

            OpenAddStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) => {
                    ThemNhanVienWindow wd = new ThemNhanVienWindow();
                    wd.ShowDialog();

                });
            OpenEditStaffCommand = new RelayCommand<object>((p) => { return true; }, 
                (p) => {
                    SuaNhanVienWindow wd = new SuaNhanVienWindow();
                    wd.FullName.Text = SelectedItem.Name;
                    wd.Gender.Text = SelectedItem.Gender;
                    wd.Date.Text = SelectedItem.BirthDate.ToString();
                    wd.Phone.Text = SelectedItem.PhoneNumber.ToString();
                    wd.Role.Text = SelectedItem.Role;
                    wd.StartDate.Text = SelectedItem.StartingDate.ToString();
                    wd.TaiKhoan.Text = SelectedItem.Username;
                    wd.MatKhau.Text = SelectedItem.Password;
                    wd.ShowDialog();
                });

            OpenDeleteStaffCommand = new RelayCommand<object>((p) => { return true; }, (p) => { XoaNhanVienWindow wd = new XoaNhanVienWindow(); wd.ShowDialog(); });

            OpenChangePassCommand = new RelayCommand<object>((p) => { return true; }, (p) => { DoiMatKhau wd = new DoiMatKhau(); wd.ShowDialog(); });

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
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

        public void ReloadStaffListView()
        {
            StaffList = new ObservableCollection<StaffDTO>(StaffService.Ins.GetAllStaff());
            listView.Items.Refresh();
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
