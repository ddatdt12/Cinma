using CinemaManagement.DTOs;
using CinemaManagement.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CinemaManagement.Views.Staff.ShowtimePage
{
    public partial class ShowtimePage : Page
    {
        public ShowtimePage()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;
            else
                return ((item as MovieDTO).DisplayName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MainListBox.ItemsSource).Refresh();
            CreateTextBoxFilter();
        }

        public void CreateTextBoxFilter()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MainListBox.ItemsSource);
            view.Filter = Filter;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtercbb.SelectedItem = null;
        }
    }
}
