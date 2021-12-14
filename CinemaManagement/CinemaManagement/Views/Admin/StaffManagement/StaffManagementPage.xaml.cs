using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.QuanLyNhanVienPage
{
    public partial class NhanVienPage : Page
    {
        public NhanVienPage()
        {
            InitializeComponent();
        }

        private void listView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as StaffDTO).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            result.Content = _ListView.Items.Count;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
