using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private int _SelectedPeriod;
        public int SelectedPeriod
        {
            get { return _SelectedPeriod; }
            set { _SelectedPeriod = value; OnPropertyChanged(); }
        }
















        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection pie { get; set; }
        public string[] Labels { get; set; }




        public ChartValues<double> Food { get; set; }
        public ChartValues<double> Ticket { get; set; }
        public List<string> YearSource { get; set; }


        public ICommand SelectedPeriodCM { get; set; }








        public StatisticalManagementViewModel()
        {
            YearSource = new List<string>();
            YearSource = GetYearCombobox();
            
            
            List<double> temp = new List<double>();
            List<double> temp1 = new List<double>();
            List<double> temp2 = new List<double>();
            List<double> temp3 = new List<double>();
            temp2.Add(2772);
            temp3.Add(1541);

            Food = new ChartValues<double>(temp2);
            Ticket = new ChartValues<double>(temp3);

            for (int i = 0; i < 10; i++)
            {
                temp.Add(i);
            }

            temp1.Add(2);
            temp1.Add(6);
            temp1.Add(8);
            temp1.Add(1);
            temp1.Add(8);
            temp1.Add(5);
            temp1.Add(6);
            temp1.Add(9);
            temp1.Add(5);
            temp1.Add(3);

            pie = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Ticket",
                    Values = new ChartValues<double>(temp2),
                    DataLabels = true,
                },

                new PieSeries
                {
                    Title = "Food",
                    Values = new ChartValues<double>(temp3),
                    DataLabels = true,
                },
            };
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Vé",

                    Values = new ChartValues<double>(temp),
                },
                new LineSeries
                {
                    Title = "Đồ ăn",
                    Values = new ChartValues<double>(temp1),
                }
            };


            SelectedPeriodCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (SelectedPeriod == 0)
                {

                    p.ItemsSource = null;
                    p.Items.Clear();

                    p.ItemsSource = GetYearCombobox();
                }
                else
                {
                    p.ItemsSource = null;
                    p.Items.Clear();

                    List<string> monthsource = new List<string>();
                    monthsource.Add("Tháng 1");
                    monthsource.Add("Tháng 2");
                    monthsource.Add("Tháng 3");
                    monthsource.Add("Tháng 4");
                    monthsource.Add("Tháng 5");
                    monthsource.Add("Tháng 6");
                    monthsource.Add("Tháng 7");
                    monthsource.Add("Tháng 8");
                    monthsource.Add("Tháng 9");
                    monthsource.Add("Tháng 10");
                    monthsource.Add("Tháng 11");
                    monthsource.Add("Tháng 12");

                    p.ItemsSource = monthsource;
                }
            });

        }


        public void ChangeComboboxItem()
        {
            MessageBox.Show("alo");
        }
        public List<string> GetYearCombobox()
        {
            List<string> yearsource = new List<string>();
            yearsource.Add("2019");
            yearsource.Add("2020");
            yearsource.Add("2021");
            return yearsource;
        }

    }
}
