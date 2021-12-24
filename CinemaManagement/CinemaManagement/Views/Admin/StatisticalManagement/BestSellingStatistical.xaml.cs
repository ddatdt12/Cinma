using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagement.Views.Admin.StatisticalManagement
{
    public partial class BestSellingStatistical : Page
    {
        public BestSellingStatistical()
        {
            InitializeComponent();
        }
        private void periodbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem s = (ComboBoxItem)periodbox1.SelectedItem;
            switch (s.Content.ToString())
            {
                case "Theo năm":
                    {
                        GetYearSource(Time1);
                        return;
                    }
                case "Theo tháng":
                    {
                        GetMonthSource(Time1);
                        return;
                    }
            }
        }
        private void periodbox1_Loaded(object sender, RoutedEventArgs e)
        {
            GetYearSource(Time1);
            return;
        }
        private void periodbox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem s = (ComboBoxItem)periodbox2.SelectedItem;
            switch (s.Content.ToString())
            {
                case "Theo năm":
                    {
                        GetYearSource(Time2);
                        return;
                    }
                case "Theo tháng":
                    {
                        GetMonthSource(Time2);
                        return;
                    }
            }
        }
        private void periodbox2_Loaded(object sender, RoutedEventArgs e)
        {
            GetYearSource(Time2);
            return;
        }
        public void GetYearSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            int now = -1;
            for (int i = 2020; i <= System.DateTime.Now.Year; i++)
            {
                now++;
                l.Add(i.ToString());
            }
            cbb.ItemsSource = l;
            cbb.SelectedIndex = now;
        }
        public void GetMonthSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            l.Add("Tháng 1");
            l.Add("Tháng 2");
            l.Add("Tháng 3");
            l.Add("Tháng 4");
            l.Add("Tháng 5");
            l.Add("Tháng 6");
            l.Add("Tháng 7");
            l.Add("Tháng 8");
            l.Add("Tháng 9");
            l.Add("Tháng 10");
            l.Add("Tháng 11");
            l.Add("Tháng 12");

            cbb.ItemsSource = l;
            cbb.SelectedIndex = DateTime.Today.Month - 1;
        }
    }
}
