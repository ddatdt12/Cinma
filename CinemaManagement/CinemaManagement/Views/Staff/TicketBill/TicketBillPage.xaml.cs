using CinemaManagement.ViewModel.StaffViewModel.TicketBillVM;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.Views.Staff.TicketBill
{
    public partial class TicketBillPage : Page
    {
        public TicketBillPage()
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
