using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace CinemaManagement.Views.Admin.StatisticalManagement
{

    public partial class IncomeStatistical : Page
    {
        public IncomeStatistical()
        {
            InitializeComponent();

        }

        private void periodbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem s = (ComboBoxItem)periodbox.SelectedItem;
            switch (s.Content.ToString())
            {
                case "Theo năm":
                    {
                        GetYearSource(Timebox);
                        return;
                    }
                case "Theo tháng":
                    {
                        GetMonthSource(Timebox);
                        return;
                    }
            }
        }
        private void periodbox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GetYearSource(Timebox);
        }
        public void GetYearSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            l.Add("2019");
            l.Add("2020");
            l.Add("2021");

            cbb.ItemsSource = l;
            cbb.SelectedIndex = 2;
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
            cbb.SelectedIndex = 0;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!string.IsNullOrEmpty(tb.Text))
            {
                if (tb.Text.StartsWith("-"))
                    tb.Foreground = new SolidColorBrush(Colors.Red);
                else
                    tb.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!string.IsNullOrEmpty(tb.Text))
            {
                if (tb.Text.StartsWith("-"))
                    tb.Foreground = new SolidColorBrush(Colors.Green);
                else
                    tb.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
