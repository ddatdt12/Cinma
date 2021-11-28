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

namespace CinemaManagement.Views.Admin.VoucherManagement.AddWindow
{

    public partial class AddVoucher : Page
    {
        private List<CheckBox> AllCheckBox = new List<CheckBox>();


        public AddVoucher()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = true;
        }

        private void allcheck(object sender, RoutedEventArgs e)
        {
            AllCheckBox.Add((CheckBox)sender);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in AllCheckBox)
                item.IsChecked = false;
        }
    }
}
