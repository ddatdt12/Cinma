using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using System.Windows;

namespace CinemaManagement.Views.Staff.TicketWindow
{
    /// <summary>
    /// Interaction logic for TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        public TicketWindow()
        {
            InitializeComponent();
            DataContext = new TicketWindowViewModel();
        }
    }
}
