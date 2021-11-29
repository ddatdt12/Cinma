using CinemaManagement.DTOs;
using CinemaManagement.ViewModel.AdminVM.VoucherManagementVM;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagement.Views.Admin.VoucherManagement.AddWindow
{

    public partial class AddVoucher : Page
    {
        public static List<CheckBox> AllCheckBox = new List<CheckBox>();
        public static CheckBox topcheck;
        public static ComboBox _cbb;

        public AddVoucher()
        {
            InitializeComponent();
            topcheck = topcheckbox;
            _cbb = cbb;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = false;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (topcheck is null) return;
            topcheck.IsChecked = false;
        }
        private void allcheck_load(object sender, System.EventArgs e)
        {
            AllCheckBox.Add((CheckBox)sender);
        }
    }
}
