using CinemaManagement.Views.Admin.Import_ExportManagement;

namespace CinemaManagement.ViewModel.AdminVM.Import_ExportManagementVM
{
    public partial class Import_ExportManagementViewModel : BaseViewModel
    {
        public void LoadBillDetailData(ExportDetail w)
        {
            w._moviename.Content = BillDetail.TicketInfo.movieName;
            w._price.Content = (BillDetail.TicketInfo.TotalPriceTicket / BillDetail.TicketInfo.seats.Count).ToString();
            w._time.Content = BillDetail.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            w._totalticket.Content = BillDetail.TicketInfo.TotalPriceTicket;
            decimal sum = 0;
            foreach (var item in BillDetail.ProductBillInfoes)
            {
                sum += item.Quantity * item.PricePerItem;
            }
            w._totalproduct.Content = sum;
        }
    }
}
