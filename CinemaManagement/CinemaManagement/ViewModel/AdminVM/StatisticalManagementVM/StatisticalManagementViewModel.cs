using CinemaManagement.Views.Admin.StatisticalManagement;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }
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

        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadAllStatisticalCM { get; set; }
        public ICommand LoadRankStatisticalCM { get; set; }







        public StatisticalManagementViewModel()
        {

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

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                p.Content = new IncomeStatistical();
            });
            StoreButtonNameCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ButtonView = p;
                p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#fafafa");
                p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);

            });
            LoadAllStatisticalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new IncomeStatistical();

            });
            LoadRankStatisticalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new RankingStatistical();
            });
        }

        public void ChangeView(Card p)
        {
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f0f2f5");
            ButtonView.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth2);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#fafafa");
            p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);
        }
    }
}
