using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementViewModel : BaseViewModel
    {
        private SeriesCollection _InComeData;
        public SeriesCollection InComeData
        {
            get { return _InComeData; }
            set { _InComeData = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedIncomePeriod;
        public ComboBoxItem SelectedIncomePeriod
        {
            get { return _SelectedIncomePeriod; }
            set { _SelectedIncomePeriod = value; OnPropertyChanged(); ChangeIncomePeriod(); }
        }
        
        private string _SelectedIncomeTime;
        public string SelectedIncomeTime
        {
            get { return _SelectedIncomeTime; }
            set { _SelectedIncomeTime = value; OnPropertyChanged(); ChangeIncomePeriod(); }
        }

        private double _TrueIncome;
        public double TrueIncome
        {
            get { return _TrueIncome; }
            set { _TrueIncome = value; OnPropertyChanged(); }
        }

        private double _TotalIn;
        public double TotalIn
        {
            get { return _TotalIn; }
            set { _TotalIn = value; OnPropertyChanged(); }
        }

        private double _TotalOut;
        public double TotalOut
        {
            get { return _TotalOut; }
            set { _TotalOut = value; OnPropertyChanged(); }
        }

        private double _TotalInPc;  //this is for the horizontial bar, just for displaying
        public double TotalInPc
        {
            get { return _TotalInPc; }
            set { _TotalInPc = value; OnPropertyChanged(); }
        }

        private double _TotalOutPc;
        public double TotalOutPc
        {
            get { return _TotalOutPc; }
            set { _TotalOutPc = value; OnPropertyChanged(); }
        }
        private int _LabelMaxValue;

        public int LabelMaxValue
        {
            get { return _LabelMaxValue; }
            set { _LabelMaxValue = value; OnPropertyChanged(); }
        }




        public void ChangeIncomePeriod()
        {
            if (SelectedIncomePeriod != null)
            {
                switch (SelectedIncomePeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                LoadIncomeByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                LoadIncomeByMonth();
                            }

                            return;
                        }
                }
            }
        }
        public void LoadIncomeByYear()
        {
            LabelMaxValue = 12;
            List<double> Thu = new List<double>();
            List<double> Chi = new List<double>();
            Thu.Add(0); Chi.Add(0);

            Random rd = new Random();

            for (int i = 0; i < 12; i++)
            {
                Thu.Add(rd.Next(100, 1000));
                Chi.Add(rd.Next(100, 1000));
            }
            CalculateTrueIncome(Thu, Chi);

            InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(Thu),
                Fill = Brushes.Transparent
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(Chi),
                Fill = Brushes.Transparent
            }
            };

        }
        public void LoadIncomeByMonth()
        {
            LabelMaxValue = 30;
            List<double> Thu = new List<double>();
            List<double> Chi = new List<double>();
            Random rd = new Random();

            for (int i = 0; i < 30; i++)
            {
                Thu.Add(rd.Next(100, 1000));
                Chi.Add(rd.Next(100, 1000));
            }
            CalculateTrueIncome(Thu, Chi);

            InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(Thu),
                Fill = Brushes.Transparent,
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(Chi),
                Fill = Brushes.Transparent,
            }
            };

        }
        public void CalculateTrueIncome(List<double> l1, List<double> l2)
        {

            TotalIn = TotalOut = 0;

            foreach (double item in l1)
                TotalIn += item;
            foreach (double item in l2)
                TotalOut += item;
            TrueIncome = TotalIn - TotalOut;

            FindMaxPercentage();
        }
        public void FindMaxPercentage()
        {
            if (TotalIn >= TotalOut)
            {
                TotalInPc = 100;
                TotalOutPc = TotalOut / TotalIn * 100;
            }
            else
            {
                TotalOutPc = 100;
                TotalInPc = TotalIn / TotalOut * 100;
            }
        }
    }
}
