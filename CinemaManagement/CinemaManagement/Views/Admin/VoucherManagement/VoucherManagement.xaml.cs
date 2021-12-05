using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace CinemaManagement.Views.Admin.VoucherManagement
{
    public partial class VoucherManagement : Page
    {
        public VoucherManagement()
        {
            InitializeComponent();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(VoucherListView.ItemsSource);
            view.Filter = Filter;
            result.Content = VoucherListView.Items.Count;
            CollectionViewSource.GetDefaultView(VoucherListView.ItemsSource).Refresh();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;
            else
                return ((item as VoucherReleaseDTO).Id.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
