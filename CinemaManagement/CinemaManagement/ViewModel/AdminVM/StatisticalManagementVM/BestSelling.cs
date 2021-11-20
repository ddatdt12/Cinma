using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void LoadBestSellingData()
        {
         
            List<double> movietop = new List<double>();
            List<double> foodtop = new List<double>();
            movietop.Add(0);foodtop.Add(0);

            for (int i = 5; i > 0; i--)
            {
                movietop.Add(i);
                foodtop.Add(i);
            }

            Top5MovieData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<double>(movietop),
                },

            };
            Top5FoodData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<double>(foodtop)
                }
            };
        }
    }
}