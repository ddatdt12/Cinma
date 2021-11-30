using System.Windows.Controls;
using System.Windows.Markup;

namespace CinemaManagement.Views.Admin.ErrorManagement
{
    public partial class ErrorManagement : Page
    {


        public ErrorManagement()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
    }
}
