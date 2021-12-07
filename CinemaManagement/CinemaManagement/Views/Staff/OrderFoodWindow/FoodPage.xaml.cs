using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using System.Windows.Controls;

namespace CinemaManagement.Views.Staff.OrderFoodWindow
{
    public partial class FoodPage : Page
    {
        public FoodPage()
        {
            InitializeComponent();
            DataContext = new OrderFoodPageViewModel();
        }
    }
}
