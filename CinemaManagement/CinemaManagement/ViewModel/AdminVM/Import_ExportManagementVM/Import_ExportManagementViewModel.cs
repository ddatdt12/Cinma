using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Admin.Import_ExportManagement;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.Import_ExportManagementVM
{
    public partial class Import_ExportManagementViewModel : BaseViewModel
    {
        private DateTime _getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _getCurrentDate; }
            set { _getCurrentDate = value; }
        }
        private string _setCurrentDate;
        public string SetCurrentDate
        {
            get { return _setCurrentDate; }
            set { _setCurrentDate = value; }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); GetExportListSource("date"); }
        }
        private ComboBoxItem _SelectedItemFilter;
        public ComboBoxItem SelectedItemFilter
        {
            get { return _SelectedItemFilter; }
            set { _SelectedItemFilter = value; OnPropertyChanged(); CheckItemFilter(); }
        }
        private BillDTO _selectedTicketBill;
        public BillDTO SelectedTicketBill
        {
            get { return _selectedTicketBill; }
            set { _selectedTicketBill = value; OnPropertyChanged(); }
        }
        private BillDTO _billDetail;
        public BillDTO BillDetail
        {
            get { return _billDetail; }
            set { _billDetail = value; OnPropertyChanged(); }
        }

        private int _SelectedMonth;
        public int SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); CheckMonthFilter(); }
        }


        public int SelectedView = 0;





        public ICommand LoadImportPageCM { get; set; }
        public ICommand LoadExportPageCM { get; set; }
        public ICommand ExportFileCM { get; set; }
        public ICommand LoadInforBillCM { get; set; }



        private List<ProductReceiptDTO> _ListProduct;
        public List<ProductReceiptDTO> ListProduct
        {
            get { return _ListProduct; }
            set { _ListProduct = value; OnPropertyChanged(); }
        }

        private List<BillDTO> _ListBill;
        public List<BillDTO> ListBill
        {
            get { return _ListBill; }
            set { _ListBill = value; OnPropertyChanged(); }
        }

        public Import_ExportManagementViewModel()
        {
            GetCurrentDate = DateTime.Today;
            SelectedDate = GetCurrentDate;
            SelectedMonth = 0;
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
                GetExportListSource("date");
                ExportPage page = new ExportPage();
                p.Content = page;
               
            });
            ExportFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportToFileFunc();
            });
            LoadInforBillCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedTicketBill != null)
                {
                    try
                    {
                        BillDetail = BillService.Ins.GetBillDetails(SelectedTicketBill.Id);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }

                    if (BillDetail.TicketInfo is null)
                    {
                        ProductDetail w = new ProductDetail();
                        decimal sum = 0;
                        foreach (var item in BillDetail.ProductBillInfoes)
                        {
                            sum += item.Quantity * item.PricePerItem;
                        }
                        w._totalproduct.Content = sum;
                        w.ShowDialog();
                    }
                    else if (BillDetail.ProductBillInfoes.Count == 0)
                    {
                        TicketDetail w = new TicketDetail();
                        w._moviename.Content = BillDetail.TicketInfo.movieName;
                        w._price.Content = BillDetail.TicketInfo.PricePerTicketStr;
                        w._time.Content = BillDetail.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                        w._totalticket.Content = BillDetail.TicketInfo.TotalPriceTicketStr;
                        w.ShowDialog();

                    }
                    else if (BillDetail.TicketInfo != null && BillDetail.ProductBillInfoes.Count != 0)
                    {
                        ExportDetail w = new ExportDetail();
                        w._moviename.Content = BillDetail.TicketInfo.movieName;
                        w._price.Content = BillDetail.TicketInfo.PricePerTicketStr;
                        w._time.Content = BillDetail.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                        w._totalticket.Content = BillDetail.TicketInfo.TotalPriceTicketStr;
                        w.ShowDialog();
                    }
                }
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
                                foreach (var item in ListProduct)
                                {

                                    ws.Cells[i2, 1] = item.Id;
                                    ws.Cells[i2, 2] = item.ProductName;
                                    ws.Cells[i2, 3] = item.Quantity;
                                    ws.Cells[i2, 4] = item.ImportPrice;
                                    ws.Cells[i2, 5] = item.StaffName;
                                    ws.Cells[i2, 6] = item.CreatedAt;

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
                                ws.Cells[1, 5] = "Tổng giá";
                                ws.Cells[1, 6] = "Giảm giá";
                                ws.Cells[1, 7] = "Sau giảm giá";

                                int i2 = 2;
                                foreach (var item in ListBill)
                                {

                                    ws.Cells[i2, 1] = item.Id;
                                    ws.Cells[i2, 2] = item.CreatedAt;
                                    ws.Cells[i2, 3] = item.CustomerName;
                                    ws.Cells[i2, 4] = item.PhoneNumber;
                                    ws.Cells[i2, 5] = item.OriginalTotalPriceStr;
                                    ws.Cells[i2, 6] = item.DiscountPriceStr;
                                    ws.Cells[i2, 7] = item.TotalPriceStr;

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
            ListProduct = new List<ProductReceiptDTO>(ProductReceiptService.Ins.GetProductReceipt());
        }
        public void GetExportListSource(string s = "")
        {
            ListBill = new List<BillDTO>();
            switch (s)
            {
                case "date":
                    {
                        ListBill = new  List<BillDTO>(BillService.Ins.GetBillByDate(SelectedDate));
                        return;
                    }
                case "":
                    {
                        ListBill = new List<BillDTO>(BillService.Ins.GetAllBill());
                        return;
                    }
                case "month":
                    {
                        CheckMonthFilter();
                        return;
                    }

            }
        }
        public void CheckItemFilter()
        {
            switch (SelectedItemFilter.Content.ToString())
            {
                case "Toàn bộ":
                    {
                        GetExportListSource("");
                        return;
                    }
                case "Theo ngày":
                    {
                        GetExportListSource("date");
                        return;
                    }
                case "Theo tháng":
                    {
                        GetExportListSource("month");
                        return;
                    }
            }
        }
        public void CheckMonthFilter()
        {
            ListBill = new List<BillDTO>(BillService.Ins.GetBillByMonth(SelectedMonth + 1));
        }
    }
}
