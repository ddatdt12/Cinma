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
