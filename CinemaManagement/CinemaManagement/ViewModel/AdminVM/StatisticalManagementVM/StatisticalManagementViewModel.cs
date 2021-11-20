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

        public SeriesCollection pie { get; set; }
        public SeriesCollection pie2 { get; set; }




        public ChartValues<double> Food { get; set; }
        public ChartValues<double> Ticket { get; set; }

        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadAllStatisticalCM { get; set; }
        public ICommand LoadRankStatisticalCM { get; set; }
        public ICommand LoadBestSellingCM { get; set; }







        public StatisticalManagementViewModel()
        {
          
            //List<double> temp = new List<double>();
            //List<double> temp1 = new List<double>();
            //List<double> temp2 = new List<double>();
            //List<double> temp3 = new List<double>();
            //temp2.Add(2772);
            //temp3.Add(1541);

            //Food = new ChartValues<double>(temp2);
            //Ticket = new ChartValues<double>(temp3);

            //pie = new SeriesCollection
            //{
            //    new PieSeries
            //    {
            //        Title = "Ticket",
            //        Values = new ChartValues<double>(temp2),
            //        DataLabels = true,
            //    },

            //    new PieSeries
            //    {
            //        Title = "Food",
            //        Values = new ChartValues<double>(temp3),
            //        DataLabels = true,
            //    },
            //};
            //pie = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Values = new ChartValues<double>(temp1),
            //        Fill = Brushes.Transparent,
            //        StrokeThickness = 0.5,
            //    },
            //};
            //pie2 = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Values = new ChartValues<double>(temp1),
            //        Stroke = Brushes.OrangeRed,
            //        Fill = Brushes.Transparent,
            //        StrokeThickness = 0.5,
            //    },
            //};

            //InComeByMonth = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Thu",

            //        Values = new ChartValues<double>(temp),
            //    },
            //    new LineSeries
            //    {
            //        Title = "Chi",
            //        Values = new ChartValues<double>(temp1),
            //    }
            //};

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
            LoadBestSellingCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new BestSellingStatistical();
                LoadBestSellingData();
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
