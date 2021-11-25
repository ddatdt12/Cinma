using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        private ObservableCollection<int> listMiniVoucher;
        public ObservableCollection<int> ListMiniVoucher
        {
            get { return listMiniVoucher; }
            set { listMiniVoucher = value; OnPropertyChanged(); }
        }


        private int? selectedWaitingVoucher;
        public int? SelectedWaitingVoucher
        {
            get { return selectedWaitingVoucher; }
            set { selectedWaitingVoucher = value; OnPropertyChanged(); }
        }



        public ICommand LoadAddWindowCM { get; set; }
        public ICommand LoadAddInforCM { get; set; }
        public ICommand LoadAddVoucherCM { get; set; }
        public ICommand LoadAddMiniVoucherCM { get; set; }
        public ICommand LoadAddListMiniVoucherCM { get; set; }
        public ICommand MoreVoucherCM { get; set; }
        public ICommand LessVoucherCM { get; set; }



        public void LessVoucherFunc()
        {
            foreach(int item in ListMiniVoucher.ToList<int>())
            {
                if (item == SelectedWaitingVoucher)
                {
                    ListMiniVoucher.Remove(item);
                }
            }
        }
    }
}
