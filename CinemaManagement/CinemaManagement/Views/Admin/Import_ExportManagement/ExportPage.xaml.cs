using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace CinemaManagement.Views.Admin.Import_ExportManagement
{
    /// <summary>
    /// Interaction logic for ExportPage.xaml
    /// </summary>
    public partial class ExportPage : Page
    {
        int indexFilter = 0;
        public ExportPage()
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

            switch (indexFilter)
            {
                case 0:
                    return ((item as MovieDTO).Country.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case 1:
                    return ((item as MovieDTO).DisplayName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case 2:
                    return ((item as MovieDTO).Image.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                default:
                    return ((item as MovieDTO).Country.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indexFilter = cbbFilter.SelectedIndex;
        }
    }
}
