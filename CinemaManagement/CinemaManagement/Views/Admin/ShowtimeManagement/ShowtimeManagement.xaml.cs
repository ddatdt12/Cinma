using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
namespace CinemaManagement.Views.Admin.ShowtimeManagementVM
{

    public partial class ShowtimeManagement : Page
    {
        public ShowtimeManagement()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(_FilterBox.Text))
                return true;
            else
                return ((item as MovieDTO).DisplayName.IndexOf(_FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ShowtimeListview.ItemsSource);
            view.Filter = Filter;
            result.Content = ShowtimeListview.Items.Count;
            CollectionViewSource.GetDefaultView(ShowtimeListview.ItemsSource).Refresh();
        }

        private void all_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _FilterBox.Text = "";
        }
    }
}
