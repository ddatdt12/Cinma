using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class BillDTO
    {
        public BillDTO()
        {
            ProductBillInfoes = new List<ProductBillInfoDTO>();
            TicketBillInfoes = new List<TicketBillInfoDTO>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public  CustomerDTO Customer { get; set; }
        public  StaffDTO Staff { get; set; }
        public  IList<ProductBillInfoDTO> ProductBillInfoes { get; set; }
        public  IList<TicketBillInfoDTO> TicketBillInfoes { get; set; }
    }
}
