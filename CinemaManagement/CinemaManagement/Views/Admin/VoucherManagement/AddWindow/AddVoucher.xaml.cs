using CinemaManagement.ViewModel.AdminVM.VoucherManagementVM;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagement.Views.Admin.VoucherManagement.AddWindow
{

    public partial class AddVoucher : Page
    {
        public static List<CheckBox> AllCheckBox = new List<CheckBox>();


        public AddVoucher()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = true;

            VoucherViewModel.WaitingMiniVoucher.Clear();
            foreach (var item in VoucherViewModel.StoreAllMini)
            {
                if (item.Status != Utils.VOUCHER_STATUS.USED)
                {
                    VoucherViewModel.WaitingMiniVoucher.Add(item);
                }
            }
            VoucherViewModel.HaveUsedVoucher();
        }

        private void allcheck(object sender, RoutedEventArgs e)
        {
            AllCheckBox.Add((CheckBox)sender);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = false;
            VoucherViewModel.WaitingMiniVoucher.Clear();
            VoucherViewModel.HaveUsed = false;
        }

        private void allcheck_Click(object sender, RoutedEventArgs e)
        {
            int idx = -1;
            CheckBox cb = sender as CheckBox;
            if (AllCheckBox.Contains(cb))
                idx = AllCheckBox.IndexOf(cb);

            if (idx != -1)
            {
                if (VoucherViewModel.WaitingMiniVoucher.Contains(VoucherViewModel.StoreAllMini[idx]))
                {
                    VoucherViewModel.WaitingMiniVoucher.Remove(VoucherViewModel.StoreAllMini[idx]);
                }
                else
                {
                    VoucherViewModel.WaitingMiniVoucher.Add(VoucherViewModel.StoreAllMini[idx]);
                }
                VoucherViewModel.HaveUsedVoucher();
            }
        }

        private void releasebtn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (toolt is null) return;

            Button btn = sender as Button;

            if (btn.IsEnabled == false)
            {
                toolt.ToolTip = "Tồn tại voucher đã phát hành!";
                toolt.Visibility = Visibility.Visible;
            }
            else
            {
                toolt.ToolTip = "";
                toolt.Visibility = Visibility.Collapsed;
            }
        }
    }
}
