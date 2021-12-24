using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.CustomerManagement;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.CustomerManagementVM
{
    public class CustomerManagementViewModel : BaseViewModel
    {
        ListView listView;
        private Label _ResultLbl;
        public Label ResultLbl
        {
            get { return _ResultLbl; }
            set { _ResultLbl = value; OnPropertyChanged(); }
        }


        #region Biến lưu dữ liệu thêm

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { _Fullname = value; OnPropertyChanged(); }
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

        #endregion

        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {

            get => _customerList;
            set
            {
                _customerList = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _SelectedPeriod;
        public ComboBoxItem SelectedPeriod
        {
            get { return _SelectedPeriod; }
            set { _SelectedPeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedTime;
        public string SelectedTime
        {
            get { return _SelectedTime; }
            set { _SelectedTime = value; OnPropertyChanged(); }
        }
        int selectedyear;

        private bool _IsGettingSource;
        public bool IsGettingSource
        {
            get { return _IsGettingSource; }
            set { _IsGettingSource = value; OnPropertyChanged(); }
        }

        public ICommand GetListViewCommand { get; set; }


        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        public ICommand OpenEditCustomerCM { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand ChangePeriodCM { get; set; }
        public ICommand SaveResultNameCM { get; set; }



        private CustomerDTO _SelectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public static Grid MaskName { get; set; }


        //Loading variable
        private bool IsSaving = false;
        public CustomerManagementViewModel()
        {
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
                {
                    listView = p;
                });
            EditCustomerCommand = new RelayCommand<Window>((p) =>
            {
                if (IsSaving)
                {
                    return false;
                }
                return true;
            }, async (p) =>
                {
                    IsSaving = true;

                    await EditCustomer(p);

                    IsSaving = false;
                });

            DeleteCustomerCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
                 {
                     MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", "Bạn có chắc muốn xoá khách hàng này không?", MessageType.Warning, MessageButtons.YesNo);
                     result.ShowDialog();

                     if (result.DialogResult == true)
                     {
                         IsGettingSource = true;

                         (bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.DeleteCustomer(SelectedItem.Id);

                         IsGettingSource = false;

                         if (isSuccess)
                         {
                             LoadCustomerListView(Utils.Operation.DELETE);
                             MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                         else
                         {
                             MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                             mb.ShowDialog();
                         }
                     }
                 });

            OpenEditCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditCustomer wd = new EditCustomer();
                ResetData();
                Fullname = SelectedItem.Name;
                Phone = SelectedItem.PhoneNumber.ToString();
                Mail = SelectedItem.Email;
                MaskName.Visibility = Visibility.Visible;
                wd.ShowDialog();
            });
            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {

                if (p != null)
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            ChangePeriodCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsGettingSource = true;
                if (SelectedPeriod != null)
                {
                    switch (SelectedPeriod.Content.ToString())
                    {
                        case "Theo năm":
                            {
                                if (SelectedPeriod != null)
                                {
                                    await LoadSourceByYear();
                                    IsGettingSource = false;
                                }
                                return;
                            }
                        case "Theo tháng":
                            {
                                if (SelectedPeriod != null)
                                {
                                    await LoadSourceByMonth();
                                    IsGettingSource = false;
                                }
                                return;
                            }
                    }
                }

            });
            SaveResultNameCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                ResultLbl = p;
            });
        }

        public void LoadCustomerListView(Operation oper, CustomerDTO cus = null)
        {

            switch (oper)
            {
                case Operation.UPDATE:
                    var cusfound = CustomerList.FirstOrDefault(c => c.Id == cus.Id);
                    CustomerList[CustomerList.IndexOf(cusfound)] = cus;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < CustomerList.Count; i++)
                    {
                        if (CustomerList[i].Id == SelectedItem?.Id)
                        {
                            CustomerList.Remove(CustomerList[i]);
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
            Fullname = null;
            Phone = null;
            Mail = null;
        }

        public async Task EditCustomer(Window p)
        {
            if (!string.IsNullOrEmpty(Mail))
            {
                if (!Utils.RegexUtilities.IsValidEmail(Mail))
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Email không hợp lệ", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE);
            if (isValid)
            {
                CustomerDTO cus = new CustomerDTO();
                cus.Id = SelectedItem.Id;
                cus.Name = Fullname;
                cus.PhoneNumber = Phone;
                cus.Email = Mail;
                cus.Expense = SelectedItem.Expense;

                (bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.UpdateCustomerInfo(cus);

                if (isSuccess)
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    LoadCustomerListView(Utils.Operation.UPDATE, cus);
                    p.Close();
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", error, MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }

        private (bool valid, string error) IsValidData(Operation oper)
        {
            if (string.IsNullOrEmpty(Fullname))
            {
                return (false, "Thông tin thiếu! Vui lòng bổ sung");
            }
            if (!Helper.IsPhoneNumber(Phone))
            {
                return (false, "Số điện thoại không hợp lệ");
            }
            return (true, null);
        }

        public async Task LoadSourceByYear()
        {
            if (SelectedTime is null) return;
            if (SelectedTime.Length != 4) return;
            try
            {
                CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(int.Parse(SelectedTime.ToString())));
                ResultLbl.Content = CustomerList.Count;
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
            selectedyear = int.Parse(SelectedTime.ToString());

        }
        public async Task LoadSourceByMonth()
        {
            if (SelectedTime is null) return;
            if (SelectedTime.ToString().Length == 4) return;
            try
            {
                CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, int.Parse(SelectedTime.ToString().Remove(0, 6))));
                ResultLbl.Content = CustomerList.Count;
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
        }
    }
}
