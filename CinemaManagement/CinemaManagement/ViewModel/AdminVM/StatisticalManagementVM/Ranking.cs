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
            set { _SelectedRankingPeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedRankingTime;
        public string SelectedRankingTime
        {
            get { return _SelectedRankingTime; }
            set { _SelectedRankingTime = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedRankingPeriod2;
        public ComboBoxItem SelectedRankingPeriod2
        {
            get { return _SelectedRankingPeriod2; }
            set { _SelectedRankingPeriod2 = value; OnPropertyChanged(); }
        }

        private string _SelectedRankingTime2;
        public string SelectedRankingTime2
        {
            get { return _SelectedRankingTime2; }
            set { _SelectedRankingTime2 = value; OnPropertyChanged(); }
        }




        public async Task ChangeRankingPeriod()
        {
            if (SelectedRankingPeriod != null)
            {
                switch (SelectedRankingPeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByMonth();
                            }

                            return;
                        }
                }
            }
        }
        public async Task LoadRankingByYear()
        {
            if (SelectedRankingTime.Length != 4) return;
            try
            {
                (List<CustomerDTO> Top5Cus, decimal TicketExpenseOfTop1, decimal ProductExpenseOfTop1) = await StatisticsService.Ins.GetTop5CustomerExpenseByYear(int.Parse(SelectedRankingTime));
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
            catch (Exception e)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống " + e.Message, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }




        }
        public async Task LoadRankingByMonth()
        {
            if (SelectedRankingTime.Length == 4) return;
            try
            {
                (List<CustomerDTO> Top5Cus, decimal TicketExpenseTop1Cus, decimal ProductExpenseTop1Cus) = await StatisticsService.Ins.GetTop5CustomerExpenseByMonth(int.Parse(SelectedRankingTime.Remove(0, 6)));
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
            catch (Exception)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống ", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }


        }

        public async Task ChangeRankingPeriod2()
        {
            if (SelectedRankingPeriod2 != null)
            {
                switch (SelectedRankingPeriod2.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByYear2();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByMonth2();
                            }

                            return;
                        }
                }
            }
        }
        public async Task LoadRankingByYear2()
        {
            if (SelectedRankingTime2.Length != 4) return;
            try
            {
                Top5Staff = await StatisticsService.Ins.GetTop5ContributionStaffByYear(int.Parse(SelectedRankingTime2));
            }
            catch (Exception)
            {

                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống ", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }

        }
        public async Task LoadRankingByMonth2()
        {
            if (SelectedRankingTime2.Length == 4) return;
            try
            {
                Top5Staff = await StatisticsService.Ins.GetTop5ContributionStaffByMonth(int.Parse(SelectedRankingTime2.Remove(0, 6)));
            }
            catch (Exception)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống ", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }

        }
    }
}
