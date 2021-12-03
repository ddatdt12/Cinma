using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;
            else
                return ((item as VoucherDTO).Code.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Listviewmini.ItemsSource);
            view.Filter = Filter;
            result.Content = Listviewmini.Items.Count;
            CollectionViewSource.GetDefaultView(Listviewmini.ItemsSource).Refresh();
        }
    }
}
