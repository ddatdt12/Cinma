using CinemaManagement.ViewModel.StaffViewModel.TicketBillVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaManagement.Views.Staff.TicketBill
{
    /// <summary>
    /// Interaction logic for TicketBillOnlyFoodPage.xaml
    /// </summary>
    public partial class TicketBillOnlyFoodPage : Page
    {
        public TicketBillOnlyFoodPage()
        {
            InitializeComponent();
            DataContext = new TicketBillViewModel();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
