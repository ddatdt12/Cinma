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

namespace CinemaManagement.Views.Admin.FoodManagementPage
{
    /// <summary>
    /// Interaction logic for EditFoodWindow.xaml
    /// </summary>
    public partial class EditFoodWindow : Window
    {
        public EditFoodWindow()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }
    }
}
