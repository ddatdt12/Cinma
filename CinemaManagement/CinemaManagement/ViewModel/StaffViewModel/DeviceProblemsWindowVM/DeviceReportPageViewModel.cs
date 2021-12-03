using CinemaManagement.DTOs;
using CinemaManagement.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.DeviceProblemsWindowVM
{
    public class DeviceReportPageViewModel: BaseViewModel
    {
        public ICommand LoadPageCommand { get; set; }
        public ICommand FilterListErrorCommand { get; set; }


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

        MessageBoxCustom Message;
        public string FilterListError { get; set; }
        public DeviceReportPageViewModel()
        {
            Demo();

            LoadPageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                Message = new MessageBoxCustom("Thông báo", "Load Page thành công", MessageType.Info, MessageButtons.OK);
                Message.ShowDialog();
            });
        }

        void Demo()
        {
            //ListError.Add(new TroubleDTO() { Id = "1", Image = "C:/Users/DELL/Downloads/Money.jpg", Title = "Mô tả 1", RepairCost = 10000, SubmittedAt = DateTime.Now, Status = "Chờ tiếp nhận" });
            //ListError.Add(new TroubleDTO() { Id = "2", Image = "C:/Users/DELL/Downloads/Money.jpg", Title = "Mô tả 2", RepairCost = 10000, SubmittedAt = DateTime.Now, Status = "Chờ tiếp nhận" });
            //ListError.Add(new TroubleDTO() { Id = "3", Image = "C:/Users/DELL/Downloads/Money.jpg", Title = "Mô tả 3", RepairCost = 10000, SubmittedAt = DateTime.Now, Status = "Chờ tiếp nhận" });
            //ListError.Add(new TroubleDTO() { Id = "4", Image = "C:/Users/DELL/Downloads/Money.jpg", Title = "Mô tả 4", RepairCost = 10000, SubmittedAt = DateTime.Now, Status = "Chờ tiếp nhận" });
        }
    }
}
