using CinemaManagement.DTOs;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CinemaManagement.Views.Admin.Import_ExportManagement
{

    public partial class Import_Export : Page
    {
        List<MovieDTO> ListSource = new List<MovieDTO>();

        
        public Import_Export()
        {
            InitializeComponent();
        }

    }
}
