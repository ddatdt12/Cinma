using CinemaManagement.Views.Admin.StatisticalManagement;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Threading.Tasks;
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

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }


        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadAllStatisticalCM { get; set; }
        public ICommand LoadRankStatisticalCM { get; set; }
        public ICommand LoadBestSellingCM { get; set; }
        public ICommand ChangeBestSellPeriodCM { get; set; }
        public ICommand ChangeBestSellPeriod2CM { get; set; }
        public ICommand ChangeIncomePeriodCM { get; set; }
        public ICommand ChangeRankingPeriodCM { get; set; }
        public ICommand ChangeRankingPeriod2CM { get; set; }



        public StatisticalManagementViewModel()
        {

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
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
            });
            ChangeBestSellPeriodCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeBestSellPeriod();
                IsLoading = false;
            });
            ChangeBestSellPeriod2CM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeBestSellPeriod2();
                IsLoading = false;
            });
            ChangeIncomePeriodCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeIncomePeriod();
                IsLoading = false;
            });
            ChangeRankingPeriodCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeRankingPeriod();
                IsLoading = false;
            });
            ChangeRankingPeriod2CM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeRankingPeriod2();
                IsLoading = false;
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
