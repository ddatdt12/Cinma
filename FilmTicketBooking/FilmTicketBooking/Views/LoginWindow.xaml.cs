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

namespace FilmTicketBooking
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        static public List<Account> ListUserAccount = new List<Account>();
        public LoginWindow()
        {
            ListUserAccount.Add(new Account("trandinhkhoi", "123456"));
            ListUserAccount.Add(new Account("lehaiphong", "123456"));
            ListUserAccount.Add(new Account("dothanhdat", "123456"));
            ListUserAccount.Add(new Account("kieubaduong", "123456"));
            ListUserAccount.Add(new Account("huynhtrungthao", "123456"));
            ListUserAccount.Add(new Account("1", "1"));


            InitializeComponent();
        }



        #region Processing Functions

        private bool IsValidateUserAccount(Account infor)
        {
            foreach (var item in ListUserAccount)
                if (infor.Username == item.Username && infor.Password == item.Password) return true;
            return false;
        }

        #endregion

















        // EVENTS FUNCTIONS


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Signin_btn_Click(object sender, RoutedEventArgs e)
        {
            Account received = new Account();
            received.Username = UserName.Text;
            received.Password = PassWord.Password;

            if (!IsValidateUserAccount(received))
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                error.Foreground = new SolidColorBrush(Colors.White);
                this.Hide();
                MainAdminWindow mainAdminWindow = new MainAdminWindow();
                mainAdminWindow.ShowDialog();
                this.Close();
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            ForgotPassWindow Forgotpage = new ForgotPassWindow();
            Forgotpage.ShowDialog();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
