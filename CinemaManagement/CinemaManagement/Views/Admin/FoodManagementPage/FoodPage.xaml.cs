using CinemaManagement.DTOs;
using CinemaManagement.ViewModel.AdminVM.FoodManagementVM;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.FoodManagementPage
{
    public partial class FoodPage : Page
    {
        public FoodPage()
        {
            InitializeComponent();
            cboxFilter.SelectedIndex = 0;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            view.Filter = Filter;
            result.Content = listBox.Items.Count;
            CollectionViewSource.GetDefaultView(listBox.ItemsSource).Refresh();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as ProductDTO).DisplayName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void cboxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBox.Text = "";

            if (FoodManagementViewModel.StoreAllFood is null) return;
            var viewmodel = (FoodManagementViewModel)DataContext;
            if (viewmodel.FilterCboxFoodCommand.CanExecute(true))
                viewmodel.FilterCboxFoodCommand.Execute(cboxFilter);
            if (result is null) return;
            result.Content = listBox.Items.Count;
        }
    }
}
