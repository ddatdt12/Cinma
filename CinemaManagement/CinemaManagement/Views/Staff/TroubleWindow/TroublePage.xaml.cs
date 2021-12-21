using System.Windows.Controls;
using System.Windows.Markup;

namespace CinemaManagement.Views.Staff.TroubleWindow
{

    public partial class TroublePage : Page
    {
        public TroublePage()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
    }
}
