using CinemaManagement.DTOs;
using CinemaManagement.ViewModel.AdminVM.Import_ExportManagementVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
