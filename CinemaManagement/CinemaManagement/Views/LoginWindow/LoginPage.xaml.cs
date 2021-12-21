using CinemaManagement.ViewModel;
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

        private void mainPage_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                FloatingPasswordBox.Focus();
                usernameTextBox.Focus();
                var viewmodel = (LoginViewModel)DataContext;
                if (viewmodel.LoginCM.CanExecute(true))
                    viewmodel.LoginCM.Execute(Error);
            }
        }

        private void loginbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FloatingPasswordBox.Focus();
            usernameTextBox.Focus();
        }
    }
}
