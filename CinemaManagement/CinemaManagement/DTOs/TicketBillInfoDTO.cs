using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.DTOs
{
    public class TicketBillInfoDTO
    {
        public int BillId { get; set; }
        public int TicketId { get; set; }
        public decimal Price { get; set; }

        public  BillDTO Bill { get; set; }
        public  TicketDTO Ticket { get; set; }
    }
}
