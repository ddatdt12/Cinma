using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaManagement.Views.Staff.ShowtimePage
{
    /// <summary>
    /// Interaction logic for ShowtimePage.xaml
    /// </summary>
    public partial class ShowtimePage : Page
    {
        public ShowtimePage()
        {
            InitializeComponent();
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
    }
}
