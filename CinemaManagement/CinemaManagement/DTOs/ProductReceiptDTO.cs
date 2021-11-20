using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class ProductReceiptDTO
    {
        public ProductReceiptDTO()
        {
        }
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ImportPrice { get; set; }
        public string ImportPriceStr
        {
            get
            {
                return Helper.FormatVNMoney(ImportPrice);
            }
        }
        public int Quantity { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
