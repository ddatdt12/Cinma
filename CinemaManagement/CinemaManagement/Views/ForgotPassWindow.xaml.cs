using System.Windows;
using System.Windows.Input;

namespace CinemaManagement
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

        private void forgotpasswindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
