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

        #region Biến lưu dữ liệu thêm

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { _Fullname = value; OnPropertyChanged(); }
        }

        private System.DateTime _SignAt;
        public System.DateTime SignAt
        {
            get { return _SignAt; }
            set { _SignAt = value; OnPropertyChanged(); }
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

        public ICommand GetListViewCommand { get; set; }


        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        public ICommand OpenEditCustomerCM { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }





        private CustomerDTO _SelectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public static Grid MaskName { get; set; }


        public CustomerManagementViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetNewCustomer());
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                    throw;
                }
            });
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; },
                (p) =>
                {
                    listView = p;
                });
            EditCustomerCommand = new RelayCommand<Window>((p) => { return true; },
                async (p) =>
                {
                    await EditCustomer(p);
                });

            DeleteCustomerCommand = new RelayCommand<Window>((p) => { return true; },
                 async (p) =>
                 {
                     MessageBoxCustom result = new MessageBoxCustom("Cảnh báo", "Bạn có chắc muốn xoá khách hàng này không?", MessageType.Warning, MessageButtons.YesNo);
                     result.ShowDialog();

                     if (result.DialogResult == true)
                     {
                         //(bool successDelete, string messageFromDelete) = await CustomerService.Ins.DeleteCustomer(SelectedItem.Id);
                         //if (successDelete)
                         //{
                         //    LoadCustomerListView(Utils.Operation.DELETE);
                         //    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromDelete, MessageType.Success, MessageButtons.OK);
                         //    mb.ShowDialog();
                         //}
                         //else
                         //{
                         //    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromDelete, MessageType.Error, MessageButtons.OK);
                         //    mb.ShowDialog();
                         //}
                     }
                 });

            OpenEditCustomerCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    EditCustomer wd = new EditCustomer();
                    ResetData();
                    wd._FullName.Text = SelectedItem.Name;
                    //wd.Date.Text = SelectedItem.SignAt.ToString();
                    wd._Phone.Text = SelectedItem.PhoneNumber.ToString();
                    wd._Mail.Text = SelectedItem.Email;
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
            }
            );
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
        }

        public void LoadCustomerListView(Operation oper, CustomerDTO cus = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    CustomerList.Add(cus);
                    break;
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
            SignAt = DateTime.Now;
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
                //cus.SignAt = SignAt;
                cus.Email = Mail;
                //(bool successUpdate, string messageFromUpdate) = await CustomerService.Ins.Update(cus);

                //if (successUpdate)
                //{
                //    MaskName.Visibility = Visibility.Collapsed;
                //    LoadCustomerListView(Utils.Operation.UPDATE, cus);
                //    p.Close();
                //    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                //    mb.ShowDialog();
                //}
                //else
                //{
                //    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                //    mb.ShowDialog();
                //}
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
    }
}
