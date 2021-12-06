using System.Windows.Controls;
using System.Windows.Markup;

namespace CinemaManagement.Views.Staff.DeviceProblemsWindow
{

    public partial class DeviceReportPage : Page
    {
        public DeviceReportPage()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
    }
}
