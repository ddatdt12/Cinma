using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementViewModel : BaseViewModel
    {
        private List<CustomerDTO> top5Customer;
        public List<CustomerDTO> Top5Customer
        {
            get { return top5Customer; }
            set { top5Customer = value; OnPropertyChanged(); }
        }

        private List<StaffDTO> top5Staff;
        public List<StaffDTO> Top5Staff
        {
            get { return top5Staff; }
            set { top5Staff = value; OnPropertyChanged(); }
        }

        private SeriesCollection _CustomerExpe;
        public SeriesCollection CustomerExpe
        {
            get { return _CustomerExpe; }
            set { _CustomerExpe = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedRankingPeriod;
        public ComboBoxItem SelectedRankingPeriod
        {
            get { return _SelectedRankingPeriod; }
            set { _SelectedRankingPeriod = value; OnPropertyChanged(); ChangeRankingPeriod(); }
        }

        private string _SelectedRankingTime;
        public string SelectedRankingTime
        {
            get { return _SelectedRankingTime; }
            set { _SelectedRankingTime = value; OnPropertyChanged(); ChangeRankingPeriod(); }
        }

        private ComboBoxItem _SelectedRankingPeriod2;
        public ComboBoxItem SelectedRankingPeriod2
        {
            get { return _SelectedRankingPeriod2; }
            set { _SelectedRankingPeriod2 = value; OnPropertyChanged(); ChangeRankingPeriod2(); }
        }

        private string _SelectedRankingTime2;
        public string SelectedRankingTime2
        {
            get { return _SelectedRankingTime2; }
            set { _SelectedRankingTime2 = value; OnPropertyChanged(); ChangeRankingPeriod2(); }
        }




        public void ChangeRankingPeriod()
        {
            if (SelectedRankingPeriod != null)
            {
                switch (SelectedRankingPeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime != null)
                            {
                                LoadRankingByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime != null)
                            {
                                LoadRankingByMonth();
                            }

                            return;
                        }
                }
            }
        }
        public void LoadRankingByYear()
        {
            if (SelectedRankingTime.Length != 4) return;
            (List<CustomerDTO> Top5Cus, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1) = StatisticsService.Ins.GetTop5CustomerExpenseByYear(int.Parse(SelectedRankingTime));
            Top5Customer = Top5Cus;

            CustomerExpe = new SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<decimal>{TicketExpenseOfTop1 },
                    Title = "Tiền vé",
                },
                new PieSeries
                {
                    Values = new ChartValues<decimal>{ProductExpenseOfTop1 },
                    Title = "Sản phẩm",
                }
            };


        }
        public void LoadRankingByMonth()
        {
            if (SelectedRankingTime.Length == 4) return;
            (List<CustomerDTO> Top5Cus, decimal TicketExpenseTop1Cus, decimal ProductExpenseTop1Cus) = StatisticsService.Ins.GetTop5CustomerExpenseByMonth(int.Parse(SelectedRankingTime.Remove(0, 6)));
            Top5Customer = Top5Cus;


            CustomerExpe = new SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<decimal>{TicketExpenseTop1Cus },
                    Title = "Tiền vé",
                },
                new PieSeries
                {
                    Values = new ChartValues<decimal>{ProductExpenseTop1Cus },
                    Title = "Sản phẩm",
                }
            };
        }

        public void ChangeRankingPeriod2()
        {
            if (SelectedRankingPeriod2 != null)
            {
                switch (SelectedRankingPeriod2.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                LoadRankingByYear2();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                LoadRankingByMonth2();
                            }

                            return;
                        }
                }
            }
        }
        public void LoadRankingByYear2()
        {
            if (SelectedRankingTime2.Length != 4) return;
            Top5Staff = StatisticsService.Ins.GetTop5ContributionStaffByYear(int.Parse(SelectedRankingTime2));
        }
        public void LoadRankingByMonth2()
        {
            if (SelectedRankingTime2.Length == 4) return;
            Top5Staff = StatisticsService.Ins.GetTop5ContributionStaffByMonth(int.Parse(SelectedRankingTime2.Remove(0, 6)));
        }
    }
}
