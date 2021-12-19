using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            set { _SelectedBestSellPeriod = value; OnPropertyChanged(); }
        }

        private string _selectedBestSellTime;
        public string SelectedBestSellTime
        {
            get { return _selectedBestSellTime; }
            set { _selectedBestSellTime = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedBestSellPeriod2;
        public ComboBoxItem SelectedBestSellPeriod2
        {
            get { return _SelectedBestSellPeriod2; }
            set { _SelectedBestSellPeriod2 = value; OnPropertyChanged(); }
        }

        private string _selectedBestSellTime2;
        public string SelectedBestSellTime2
        {
            get { return _selectedBestSellTime2; }
            set { _selectedBestSellTime2 = value; OnPropertyChanged(); }
        }



        public async Task ChangeBestSellPeriod()
        {
            if (SelectedBestSellPeriod != null)
            {
                switch (SelectedBestSellPeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedBestSellTime != null)
                            {
                                await LoadBestSellByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedBestSellTime != null)
                            {
                                await LoadBestSellByMonth();
                            }
                            return;
                        }
                }
            }
        }
        public async Task LoadBestSellByYear()
        {
            if (SelectedBestSellTime.Length != 4) return;
            try
            {
                Top5Movie = await Task.Run(() => StatisticsService.Ins.GetTop5BestMovieByYear(int.Parse(SelectedBestSellTime)));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }



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
                    Title = "Doanh thu"
                },
            };
        }
        public async Task LoadBestSellByMonth()
        {
            if (SelectedBestSellTime.Length == 4) return;
            try
            {
                Top5Movie = await Task.Run(() => StatisticsService.Ins.GetTop5BestMovieByMonth(int.Parse(SelectedBestSellTime.Remove(0, 6))));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }



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
                     Title = "Doanh thu"
                },

            };
        }



        public async Task ChangeBestSellPeriod2()
        {
            if (SelectedBestSellPeriod2 != null)
            {
                switch (SelectedBestSellPeriod2.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedBestSellTime2 != null)
                            {
                                await LoadBestSellByYear2();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedBestSellTime2 != null)
                            {
                                await LoadBestSellByMonth2();
                            }
                            return;
                        }
                }
            }
        }
        public async Task LoadBestSellByYear2()
        {
            if (SelectedBestSellTime2.Length != 4) return;
            try
            {
                Top5Product = await Task.Run(() => StatisticsService.Ins.GetTop5BestProductByYear(int.Parse(SelectedBestSellTime2)));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }


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
                     Title = "Doanh thu"
                },

            };
        }
        public async Task LoadBestSellByMonth2()
        {
            if (SelectedBestSellTime2.Length == 4) return;
            try
            {
                Top5Product = await Task.Run(() => StatisticsService.Ins.GetTop5BestProductByMonth(int.Parse(SelectedBestSellTime2.Remove(0, 6))));

            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }

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
                     Title = "Doanh thu"
                },
            };
        }
    }
}