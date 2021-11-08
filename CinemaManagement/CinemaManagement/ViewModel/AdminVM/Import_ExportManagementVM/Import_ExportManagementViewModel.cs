using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.Import_ExportManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.Import_ExportManagementVM
{
    public partial class Import_ExportManagementViewModel : BaseViewModel
    {
        public ICommand LoadImportPageCM { get; set; }
        public ICommand LoadExportPageCM { get; set; }
        public ICommand ExportFileCM { get; set; }




        private List<MovieDTO> _ListSource;

        public List<MovieDTO> ListSource
        {
            get { return _ListSource; }
            set { _ListSource = value; }
        }





        public int SelectedView = 0;

        public Import_ExportManagementViewModel()
        {

            ListSource = new List<MovieDTO>();
            for (int i = 0; i < 9; i++)
            {

                MovieDTO temp = new MovieDTO
                {
                    Country = "Viet nam",
                    Description = "asdadasdasd",
                    Director = "tran khoi",
                    DisplayName = "bo gia",
                    MovieType = "kinh di",
                };
                ListSource.Add(temp);
            }



            LoadImportPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                SelectedView = 0;
                p.Content = new ImportPage();
            });
            LoadExportPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                SelectedView = 1;
                p.Content = new ExportPage();
            });
            ExportFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportToFileFunc();
            });
        }


        public void ExportToFileFunc()
        {
            switch (SelectedView)
            {
                case 0:
                    {

                        using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                                app.Visible = false;
                                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
                             
                                int i2 = 1;
                                foreach (var item in ListSource)
                                {
                                  
                                    ws.Cells[i2, 1] = item.Country;
                                    ws.Cells[i2, 2] = item.Description;
                                    ws.Cells[i2, 3] = item.Director;
                                    ws.Cells[i2, 4] = item.DisplayName;
                                    ws.Cells[i2, 5] = item.MovieType;

                                    i2++;
                                }
                                ws.SaveAs(sfd.FileName);
                                wb.Close();
                                app.Quit();
                                MessageBox.Show("Xuất file thành công");
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        break;
                    }
            }
        }
    }
}
