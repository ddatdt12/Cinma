using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.ViewModel.AdminVM.StatisticalManagementVM
{
   public partial class StatisticalManagementViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double,string> Formatter { get; set; }


        public StatisticalManagementViewModel()
        {
            List<double> temp = new List<double>();
            for (int i =0;i<30;i++)
            {
                temp.Add(i);
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Vé",
                    Values = new ChartValues<double>(temp),
                },
                new ColumnSeries
                {
                    Title = "Đồ ăn",
                    Values = new ChartValues<double>(temp),
                }
            };
        }


    }
}
