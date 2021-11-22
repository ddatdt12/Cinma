using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using System.Windows.Controls;

namespace CinemaManagement.Views.Staff.TicketWindow
{
    /// <summary>
    /// Interaction logic for TicketBookingPage.xaml
    /// </summary>
    public partial class TicketBookingPage : Page
    {
        public TicketBookingPage()
        {
            InitializeComponent();
            DataContext = new TicketWindowViewModel();
        }
    }
}
