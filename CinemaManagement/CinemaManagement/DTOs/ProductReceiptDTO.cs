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
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal ImportPrice { get; set; }
        public int Quantity { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public ProductDTO Product { get; set; }
    }
}
