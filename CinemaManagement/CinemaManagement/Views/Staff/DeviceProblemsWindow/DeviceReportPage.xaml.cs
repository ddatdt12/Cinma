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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaManagement.Views.Staff.DeviceProblemsWindow
{
    /// <summary>
    /// Interaction logic for DeviceReportPage.xaml
    /// </summary>
    public partial class DeviceReportPage : Page
    {
        public DeviceReportPage()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
    }
}
