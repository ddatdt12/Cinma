using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.Views.Admin.FoodManagementPage
{
    public partial class FoodPage : Page
    {
        public FoodPage()
        {
            InitializeComponent();
            cboxFilter.SelectedIndex = 0;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
