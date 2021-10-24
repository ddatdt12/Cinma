﻿using CinemaManagement.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace CinemaManagement.Views.Admin.QuanLySuatChieuPage
{
    /// <summary>
    /// Interaction logic for QuanLySuatChieuPage.xaml
    /// </summary>
    public partial class QuanLySuatChieuPage : Page
    {
        public QuanLySuatChieuPage()
        {
            InitializeComponent();

            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MovieListview.ItemsSource);
            //view.Filter = Filter;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MovieListview.ItemsSource).Refresh();
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
