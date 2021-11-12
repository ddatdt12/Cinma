using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace CinemaManagement.Views.Admin.Import_ExportManagement
{
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;
            else
                return ((item as MovieDTO).DisplayName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

    }
}
