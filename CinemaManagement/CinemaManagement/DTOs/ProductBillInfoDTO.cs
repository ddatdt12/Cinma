using System;

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
        public decimal TotalPrice
        {
            get
            {
                return Decimal.Truncate(Quantity * PricePerItem);
            }
        }
    }
}
