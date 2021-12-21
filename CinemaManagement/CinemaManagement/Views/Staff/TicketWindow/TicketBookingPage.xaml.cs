using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
