using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementViewModel : BaseViewModel
    {
        private SeriesCollection _Top5MovieData;
        public SeriesCollection Top5MovieData
        {
            get { return _Top5MovieData; }
            set { _Top5MovieData = value; OnPropertyChanged(); }
        }

        private SeriesCollection _Top5FoodData;
        public SeriesCollection Top5FoodData
        {
            get { return _Top5FoodData; }
            set { _Top5FoodData = value; OnPropertyChanged(); }
        }

        private List<MovieDTO> top5Movie;
        public List<MovieDTO> Top5Movie
        {
            get { return top5Movie; }
            set { top5Movie = value; OnPropertyChanged(); }
        }

        private List<ProductDTO> top5Product;
        public List<ProductDTO> Top5Product
        {
            get { return top5Product; }
            set { top5Product = value; OnPropertyChanged(); }
        }


        private ComboBoxItem _SelectedBestSellPeriod;
        public ComboBoxItem SelectedBestSellPeriod
        {
            get { return _SelectedBestSellPeriod; }
            set { _SelectedBestSellPeriod = value; OnPropertyChanged(); ChangeBestSSellPeriod(); }
        }

        private string _selectedBestSellTime;
        public string SelectedBestSellTime
        {
            get { return _selectedBestSellTime; }
            set { _selectedBestSellTime = value; OnPropertyChanged(); ChangeBestSSellPeriod(); }
        }

        private ComboBoxItem _SelectedBestSellPeriod2;
        public ComboBoxItem SelectedBestSellPeriod2
        {
            get { return _SelectedBestSellPeriod2; }
            set { _SelectedBestSellPeriod2 = value; OnPropertyChanged(); ChangeBestSSellPeriod2(); }
        }

        private string _selectedBestSellTime2;
        public string SelectedBestSellTime2
        {
            get { return _selectedBestSellTime2; }
            set { _selectedBestSellTime2 = value; OnPropertyChanged(); ChangeBestSSellPeriod2(); }
        }




        public void ChangeBestSSellPeriod()
        {
            if (SelectedBestSellPeriod != null)
            {
                switch (SelectedBestSellPeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedBestSellTime != null)
                            {
                                LoadBestSellByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedBestSellTime != null)
                            {
                                LoadBestSellByMonth();
                            }
                            return;
                        }
                }
            }
        }
        public void LoadBestSellByYear()
        {
            if (SelectedBestSellTime.Length != 4) return;
            Top5Movie = StatisticsService.Ins.GetTop5BestMovieByYear(2021);


            List<decimal> chartdata = new List<decimal>();
            chartdata.Add(0);
            for (int i = 0; i < Top5Movie.Count; i++)
            {
                chartdata.Add(Top5Movie[i].Revenue / 1000000);
            }

            Top5MovieData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<decimal>(chartdata),
                },

            };
        }
        public void LoadBestSellByMonth()
        {
            if (SelectedBestSellTime.Length == 4) return;
            Top5Movie = StatisticsService.Ins.GetTop5BestMovieByMonth(int.Parse(SelectedBestSellTime.Remove(0, 6)));


            List<decimal> chartdata = new List<decimal>();
            chartdata.Add(0);
            for (int i = 0; i < Top5Movie.Count; i++)
            {
                chartdata.Add(Top5Movie[i].Revenue / 1000000);
            }

            Top5MovieData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<decimal>(chartdata),
                },

            };
        }



        public void ChangeBestSSellPeriod2()
        {
            if (SelectedBestSellPeriod2 != null)
            {
                switch (SelectedBestSellPeriod2.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedBestSellTime2 != null)
                            {
                                LoadBestSellByYear2();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedBestSellTime2 != null)
                            {
                                LoadBestSellByMonth2();
                            }
                            return;
                        }
                }
            }
        }
        public void LoadBestSellByYear2()
        {
            if (SelectedBestSellTime2.Length != 4) return;
            Top5Product = StatisticsService.Ins.GetTop5BestProductByYear(2021);


            List<decimal> chartdata = new List<decimal>();
            chartdata.Add(0);
            for (int i = 0; i < Top5Product.Count; i++)
            {
                chartdata.Add(Top5Product[i].Revenue / 1000000);
            }

            Top5FoodData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<decimal>(chartdata),
                },

            };
        }
        public void LoadBestSellByMonth2()
        {
            if (SelectedBestSellTime2.Length == 4) return;
            Top5Product = StatisticsService.Ins.GetTop5BestProductByMonth(int.Parse(SelectedBestSellTime2.Remove(0, 6)));


            List<decimal> chartdata = new List<decimal>();
            chartdata.Add(0);
            for (int i = 0; i < Top5Product.Count; i++)
            {
                chartdata.Add(Top5Product[i].Revenue / 1000000);
            }

            Top5FoodData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<decimal>(chartdata),
                },

            };
        }
    }
}