using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using System.Windows.Controls;

namespace CinemaManagement.Views.Staff.TicketWindow
{
    public partial class TicketBookingPage : Page
    {
        public TicketBookingPage()
        {
            InitializeComponent();
            DataContext = new TicketWindowViewModel();
        }
    }
}
