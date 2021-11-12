using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.Import_ExportManagement;
using System.Collections.ObjectModel;
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




        private ObservableCollection<MovieDTO> _ListSource;

        public ObservableCollection<MovieDTO> ListSource
        {
            get { return _ListSource; }
            set { _ListSource = value;OnPropertyChanged(); }
        }


        public int SelectedView = 0;

        public Import_ExportManagementViewModel()
        {

            LoadImportPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                SelectedView = 0;
                GetImportListSource();
                ImportPage page = new ImportPage();
                p.Content = page;
            });
            LoadExportPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                SelectedView = 1;
                GetExportListSource();
                ExportPage page = new ExportPage();
                p.Content = page;
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
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                                app.Visible = false;
                                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                                ws.Cells[1, 1] = "Mã đơn";
                                ws.Cells[1, 2] = "Tên đơn";
                                ws.Cells[1, 3] = "Số lượng";
                                ws.Cells[1, 4] = "Tổng giá";
                                ws.Cells[1, 5] = "Nhân viên";
                                ws.Cells[1, 6] = "Ngày nhập";

                                int i2 = 2;
                                foreach (var item in ListSource)
                                {

                                    ws.Cells[i2, 1] = item.Country;
                                    ws.Cells[i2, 2] = item.Description;
                                    ws.Cells[i2, 3] = item.Director;
                                    ws.Cells[i2, 4] = item.Director;
                                    ws.Cells[i2, 5] = item.DisplayName;
                                    ws.Cells[i2, 6] = item.MovieType;

                                    i2++;
                                }
                                ws.SaveAs(sfd.FileName);
                                wb.Close();
                                app.Quit();

                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                                MessageBox.Show("Xuất file thành công");
                            }
                        }
                        break;
                    }
                case 1:
                    {

                        using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                                app.Visible = false;
                                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                                ws.Cells[1, 1] = "Mã đơn";
                                ws.Cells[1, 2] = "Ngày xuất";
                                ws.Cells[1, 3] = "Khách hàng";
                                ws.Cells[1, 4] = "Số điện thoại";
                                ws.Cells[1, 5] = "Số lượng vé";
                                ws.Cells[1, 6] = "Tổng giá";
                                ws.Cells[1, 7] = "Giảm giá";
                                ws.Cells[1, 8] = "Sau giảm giá";

                                int i2 = 2;
                                foreach (var item in ListSource)
                                {

                                    ws.Cells[i2, 1] = item.Country;
                                    ws.Cells[i2, 2] = item.MovieType;
                                    ws.Cells[i2, 3] = item.DisplayName;
                                    ws.Cells[i2, 4] = item.Image;
                                    ws.Cells[i2, 5] = item.Image;
                                    ws.Cells[i2, 6] = item.Director;
                                    ws.Cells[i2, 7] = item.Director;
                                    ws.Cells[i2, 8] = item.Director;
                                   

                                    i2++;
                                }
                                ws.SaveAs(sfd.FileName);
                                wb.Close();
                                app.Quit();

                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                                MessageBox.Show("Xuất file thành công");
                            }
                        }
                        break;
                    }
            }
        }
        public void GetImportListSource()
        {
            ListSource = new ObservableCollection<MovieDTO>();
            for (int i = 0; i < 9; i++)
            {

                MovieDTO temp = new MovieDTO
                {
                    Country = "088578",
                    Description = "Nhập cocacola, 7up",
                    Director = "70000000",
                    DisplayName = "Trần Khôi",
                    MovieType = "25/3/2021",
                };
                ListSource.Add(temp);
            }

        }
        public void GetExportListSource()
        {
            ListSource = new ObservableCollection<MovieDTO>();
            for (int i = 0; i < 9; i++)
            {

                MovieDTO temp = new MovieDTO
                {
                    Country = "088578",
                    Description = "Mở mắt thấy đen thui",
                    Image = "5",
                    Director = "70000000",
                    DisplayName = "Trần Khôi",
                    MovieType = "25/3/2021",
                };
                ListSource.Add(temp);
            }
        }
    }
}
