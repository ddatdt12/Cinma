using System;

namespace CinemaManagement.DTOs
{
    public class ProductBillInfoDTO
    {
        public ProductBillInfoDTO()
        {

        }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public decimal PricePerItem { get; set; }

        public virtual BillDTO Bill { get; set; }
        public virtual ProductDTO Product { get; set; }
    }
}
