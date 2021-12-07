using CinemaManagement.Utils;

namespace CinemaManagement.DTOs
{
    public class ProductBillInfoDTO
    {
        public ProductBillInfoDTO()
        {

        }
        public string BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerItem { get; set; }
        public string PricePerItemStr { get {
                return Helper.FormatDecimal(PricePerItem);
            } }
        public string TotalPriceStr
        {
            get
            {
                return Helper.FormatVNMoney(Quantity * PricePerItem);
            }
        }
    }
}
