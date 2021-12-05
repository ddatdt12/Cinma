using System.Windows.Controls;

namespace CinemaManagement.Views.LoginWindow
{
    public partial class LoginPage : Page
    {
       public static ProgressBar pgb;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ProgressBar_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            pgb = sender as ProgressBar;
        }
    }
}
