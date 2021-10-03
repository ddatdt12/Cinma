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
using System.Windows.Shapes;

namespace FilmTicketBooking
{
    /// <summary>
    /// Interaction logic for ForgotPassWindow.xaml
    /// </summary>
    public partial class ForgotPassWindow : Window
    {
        public ForgotPassWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string returnpass;
            if(!IsTrueUser(userforgot.Text, out returnpass))
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                error.Foreground = new SolidColorBrush(Colors.White);
                MessageBox.Show("Your password is: " + returnpass);
            }
        }

        private bool IsTrueUser(string value, out string returnpass)
        {
            foreach (var item in LoginWindow.ListUserAccount)
            {
                if (value == item.Username)
                {
                    if (userhint.Text == "25/03")
                    {
                        returnpass = item.Password;
                        return true;
                    }
                }
            }
            returnpass = "";
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            LoginWindow Logiwindow = new LoginWindow();
            Logiwindow.ShowDialog();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
